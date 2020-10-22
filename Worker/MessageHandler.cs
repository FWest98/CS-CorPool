using System;
using System.Linq;
using System.Threading.Tasks;
using CorPool.Mongo.DatabaseModels;
using ApiModels = CorPool.Shared.ApiModels;
using CorPool.Shared.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MongoDB.Driver.Core.Authentication;
using MongoDB.Driver.Linq;
using RabbitMQ.Client.Core.DependencyInjection;
using RabbitMQ.Client.Core.DependencyInjection.MessageHandlers;
using RabbitMQ.Client.Events;

namespace CorPool.Worker {
    public class MessageHandler : IAsyncMessageHandler {
        private readonly ILogger<MessageHandler> _logger;
        private readonly IHubContext<RideHub, IRideHubClient> _hub;
        private readonly DatabaseContext _database;

        public MessageHandler(ILogger<MessageHandler> logger, IHubContext<RideHub, IRideHubClient> hub, DatabaseContext database) {
            _logger = logger;
            _hub = hub;
            _database = database;
        }

        public async Task Handle(BasicDeliverEventArgs eventArgs, string matchingRoute) {
            var requestId = eventArgs.GetMessage();
            var request = await _database.RideRequests.AsQueryable().FirstOrDefaultAsync(s => s.Id == requestId);

            // Now we would typically do some magic to find an appropriate offer
            // but we only do a very simple random find

            // List of eligible offers
            var offers = await _database.Offers.AsQueryable()
                .Where(s => 
                    s.TenantId == request.TenantId // Same company
                    && s.UserId != request.UserId // Not your own offers
                ).ToListAsync();

            if (offers.Count == 0) {
                // No offer found
                await _hub.Clients.User(request.UserId).RideResult(null);
                _logger.LogInformation("Did not find a match");
                return;
            }

            // Find random item
            var dbOffer = offers[new Random().Next(offers.Count)];

            // Add a confirmation to this offer and store
            dbOffer.Confirmations.Add(new Confirmation {
                PickupPoint = request.From,
                UserId = request.UserId
            });
            await _database.Offers.ReplaceOneAsync(s => s.Id == dbOffer.Id, dbOffer);
            await _database.RideRequests.DeleteOneAsync(s => s.Id == requestId);

            // Return offer to SignalR
            var offer = new ApiModels.Offer(dbOffer);
            await offer.SetUsers(async userId =>
                new ApiModels.User(await _database.Users.AsQueryable().FirstOrDefaultAsync(s => s.Id == userId)));
            
            await _hub.Clients.User(request.UserId).RideResult(offer);

            _logger.LogInformation("Found a match");
        }
    }
}
