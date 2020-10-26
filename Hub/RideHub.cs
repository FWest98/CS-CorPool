using System;
using System.Threading;
using System.Threading.Tasks;
using Corpool.AspNetCoreTenant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SignalR = Microsoft.AspNetCore.SignalR;
using RabbitMQ.Client.Core.DependencyInjection.Services;

namespace CorPool.Hub {
    [Authorize]
    [Tenanted]
    public class RideHub : SignalR.Hub {
        private readonly IQueueService _queueService;
        public RideHub(IQueueService queueService) {
            _queueService = queueService;
        }

        public async Task Echo(string message) {
            Thread.Sleep(TimeSpan.FromSeconds(5));
            await Clients.Caller.SendAsync("echo", message);
            //return message;
        }

        public async Task RabbitEcho(string message) {
            // Send a message to rabbitmq for processing
            await _queueService.SendStringAsync(message, "test", "routing");
        }
    }
}
