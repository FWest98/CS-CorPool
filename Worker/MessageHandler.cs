using System.Threading.Tasks;
using CorPool.Shared.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client.Core.DependencyInjection;
using RabbitMQ.Client.Core.DependencyInjection.MessageHandlers;
using RabbitMQ.Client.Events;

namespace CorPool.Worker {
    public class MessageHandler : IAsyncMessageHandler {
        private readonly ILogger<MessageHandler> _logger;
        private readonly IHubContext<RideHub> _hub;
        public MessageHandler(ILogger<MessageHandler> logger, IHubContext<RideHub> hub) {
            _logger = logger;
            _hub = hub;
        }

        public async Task Handle(BasicDeliverEventArgs eventArgs, string matchingRoute) {
            _logger.LogInformation(eventArgs.GetMessage());
            await _hub.Clients.All.SendAsync("echo", eventArgs.GetMessage());
        }
    }
}
