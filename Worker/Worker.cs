using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client.Core.DependencyInjection.Services;

namespace CorPool.Worker {
    public class Worker : BackgroundService {
        private readonly ILogger<Worker> _logger;
        private readonly IQueueService _queueService;

        public Worker(ILogger<Worker> logger, IQueueService queueService) {
            _logger = logger;
            _queueService = queueService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
            _queueService.StartConsuming();
            while (!stoppingToken.IsCancellationRequested) {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(10000, stoppingToken);
            }
        }
    }
}
