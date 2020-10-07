using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Corpool.AspNetCoreTenant;
using CorPool.Mongo.DatabaseModels;
using RabbitMQ.Client.Core.DependencyInjection.Services;

namespace CorPool.BackEnd.Controllers {
    [Tenanted]
    public class QueueController : AbstractApiController {
        private readonly IQueueService _queueService;
        public QueueController(DatabaseContext database, IQueueService queueService) : base(database) {
            _queueService = queueService;
        }

        public async Task<string> Get(string text) {
            await _queueService.SendStringAsync(text, "test", "routing");
            return text + " queued";
        }
    }
}
