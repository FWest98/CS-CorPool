using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Corpool.AspNetCoreTenant;
using CorPool.BackEnd.Helpers.Jwt;
using CorPool.Mongo.DatabaseModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using RabbitMQ.Client.Core.DependencyInjection.Services;
using ApiModels = CorPool.Shared.ApiModels;

namespace CorPool.BackEnd.Controllers {
    [Tenanted]
    [Authorize]
    public class RideController : AbstractApiController {
        private readonly IQueueService _queueService;
        public RideController(Lazy<DatabaseContext> database, Lazy<JwtUserManager> userManager, Lazy<IDistributedCache> cache, IQueueService queueService) : base(database, userManager, cache) {
            _queueService = queueService;
        }

        [HttpGet]
        public async Task<ActionResult<List<ApiModels.Offer>>> Get() {
            // Get all confirmed rides for the current user
            var user = await User;
            var dbConfirmations = await Database.Offers.Tenanted(Tenant)
                .Where(s => s.Confirmations.Any(a => a.UserId == user.Id)).ToListAsync();

            var apiOffers = dbConfirmations.Select(s => new ApiModels.Offer(s)).ToList();
            // Set users
            await Task.WhenAll(apiOffers.Select(s => s.SetUsers(async userId => new ApiModels.User(await UserManager.FindByIdAsync(userId)))));

            return apiOffers;
        }
    }
}
