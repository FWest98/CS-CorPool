using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Corpool.AspNetCoreTenant;
using CorPool.BackEnd.Helpers.Jwt;
using CorPool.Mongo.DatabaseModels;
using Microsoft.Extensions.Caching.Distributed;
using RabbitMQ.Client.Core.DependencyInjection.Services;

namespace CorPool.BackEnd.Controllers {
    [Tenanted]
    public class QueueController : AbstractApiController {
        private readonly IQueueService _queueService;
        public QueueController(Lazy<DatabaseContext> database, Lazy<JwtUserManager> userManager, Lazy<IDistributedCache> cache, IQueueService queueService) : base(database, userManager, cache) {
            _queueService = queueService;
        }

        public async Task<string> Get(string text) {
            await _queueService.SendStringAsync(text, "corpool", "routing");
            return text + " queued";
        }
    }
}
