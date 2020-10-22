using System.Threading.Tasks;
using Corpool.AspNetCoreTenant;
using CorPool.Mongo.DatabaseModels;
using CorPool.Shared.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using RabbitMQ.Client.Core.DependencyInjection.Services;

namespace CorPool.Shared.Hubs {
    [Authorize]
    [Tenanted]
    public class RideHub : Hub<IRideHubClient> {
        private readonly IQueueService _queueService;
        private readonly IOptions<RabbitOptions> _queueOptions;
        private readonly DatabaseContext _database;

        private Tenant Tenant => Context.GetHttpContext().GetTenant<Tenant>();

        public RideHub(IQueueService queueService, IOptions<RabbitOptions> queueOptions, DatabaseContext database) {
            _queueService = queueService;
            _queueOptions = queueOptions;
            _database = database;
        }

        public async Task RideRequest(ApiModels.RideRequest request) {
            // Make a database model
            var dbRequest = new RideRequest {
                ArrivalTime = request.ArrivalTime,
                From = new Location { 
                    Description = request.From.Description,
                    Title = request.From.Title
                },
                To = new Location {
                    Description = request.To.Description,
                    Title = request.To.Title
                },
                UserId = Context.UserIdentifier,
                TenantId = Tenant.Id
            };

            // Store
            await _database.RideRequests.InsertOneAsync(dbRequest);

            // Queue
            var options = _queueOptions.Value;
            await _queueService.SendStringAsync(dbRequest.Id, options.Name, options.RoutingKey);
        }
    }
}
