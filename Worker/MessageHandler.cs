using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client.Core.DependencyInjection;
using RabbitMQ.Client.Core.DependencyInjection.MessageHandlers;
using RabbitMQ.Client.Events;

namespace Worker {
    public class MessageHandler : IAsyncMessageHandler {
        private readonly ILogger<MessageHandler> _logger;
        public MessageHandler(ILogger<MessageHandler> logger) {
            _logger = logger;
        }

        public async Task Handle(BasicDeliverEventArgs eventArgs, string matchingRoute) {
            _logger.LogInformation(eventArgs.GetMessage());
        }
    }
}
