using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

namespace CorPool.BackEnd.Controllers {
    [Tenanted]
    [Authorize]
    public class OfferController : AbstractApiController {
        public OfferController(Lazy<DatabaseContext> database, Lazy<JwtUserManager> userManager, Lazy<IDistributedCache> cache) : base(database, userManager, cache) { }

        [HttpGet]
        public async Task<ActionResult<List<ApiModels.Offer>>> Get() {
            var dbOffers = await Database.Offers.Tenanted(Tenant).ToListAsync();
            var apiOffers = dbOffers.Select(s => new ApiModels.Offer(s)).ToList();

            // Set Users
            await Task.WhenAll(apiOffers.Select(s => s.SetUsers(async userId => new ApiModels.User(await UserManager.FindByIdAsync(userId)))));

            return apiOffers;
        }

        [HttpPost]
        public async Task<ActionResult<ApiModels.Offer>> Post([FromBody] Models.OfferCreate offer) {
            // Create new offer in database
            var dbOffer = new Offer {
                ArrivalTime = offer.ArrivalTime,
                From = new Location {
                    Description = offer.From.Description,
                    Title = offer.From.Title
                },
                To = new Location {
                    Description = offer.To.Description,
                    Title = offer.To.Title
                },
                Vehicle = new Vehicle {
                    Model = offer.Vehicle.Model,
                    Color = offer.Vehicle.Color,
                    Capacity = offer.Vehicle.Capacity,
                    Brand = offer.Vehicle.Brand
                },
                TenantId = Tenant.Id,
                UserId = UserId
            };

            await Database.Offers.InsertOneAsync(dbOffer);

            // Transform back to apimodel for response
            var apiOffer = new ApiModels.Offer(dbOffer);
            await apiOffer.SetUsers(async _ => new ApiModels.User(await User));

            return apiOffer;
        }

        public class Models {
            public class OfferCreate {
                [Required]
                public ApiModels.Vehicle Vehicle { get; set; }

                [Required]
                public ApiModels.Location From { get; set; }

                [Required]
                public ApiModels.Location To { get; set; }

                [Required]
                public DateTime ArrivalTime { get; set; }
            }
        }
    }
}
