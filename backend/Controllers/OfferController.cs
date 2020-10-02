using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CorPool.BackEnd.DatabaseModels;
using CorPool.BackEnd.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace CorPool.BackEnd.Controllers {
    [Tenanted]
    [Authorize]
    public class OfferController : AbstractApiController {
        public OfferController(DatabaseContext database) : base(database) { }

        [HttpGet]
        public async Task<ActionResult<List<ApiModels.Offer>>> Get() {
            var dbOffers = await database.Offers.Tenanted(Tenant).ToListAsync();
            var apiOffers = dbOffers.Select(s => new ApiModels.Offer(s)).ToList();

            // Set Users
            await Task.WhenAll(apiOffers.Select(s => s.SetUsers(async userId => {
                var dbUser = await database.Users.Tenanted(Tenant).FirstOrDefaultAsync(a => a.Id == userId);
                return new ApiModels.User(dbUser);
            })));

            return apiOffers;
        }
    }
}
