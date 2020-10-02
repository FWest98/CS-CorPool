using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CorPool.BackEnd.ApiModels;
using CorPool.BackEnd.Attributes;
using CorPool.BackEnd.DatabaseModels;
using Microsoft.AspNetCore.Authorization;

namespace CorPool.BackEnd.Controllers {
    [Tenanted]
    [Authorize]
    public class OfferController : AbstractApiController {
        public OfferController(DatabaseContext database) : base(database) { }

        public async Task<IEnumerable<ApiModels.Offer>> Get() {
            var offers = new List<ApiModels.Offer> {
                new ApiModels.Offer {
                    Id = "NEW",
                    User = new ApiModels.User { Name = "Danny" },
                    ArrivalTime = DateTime.Now,
                    Confirmations = new List<Confirmation> {
                        new Confirmation {
                            User = new ApiModels.User { Name = "Tom" },
                            PickupPoint = new ApiModels.Location { Description = "Your house", Title = "Easy" }
                        }
                    },
                    From = new ApiModels.Location { Description = "Nijenborgh 4, Groningen", Title = "Home" },
                    To = new ApiModels.Location { Description = "The brown office", Title = "Work" },
                    Vehicle = new Vehicle { Brand = "Volvo", Capacity = 4, Color = "Black", Model = "V70" }
                },

                new ApiModels.Offer {
                    Id = "2",
                    User = new ApiModels.User { Name = "Eva" },
                    ArrivalTime = DateTime.Now,
                    Confirmations = new List<Confirmation> {
                        new Confirmation {
                            User = new ApiModels.User { Name = "Dirk" },
                            PickupPoint = new ApiModels.Location { Description = "Your house", Title = "Easy" }
                        }
                    },
                    From = new ApiModels.Location { Description = "Nijenborgh 4, Groningen", Title = "Home" },
                    To = new ApiModels.Location { Description = "The brown office", Title = "Work" },
                    Vehicle = new Vehicle { Brand = "Peugeot", Capacity = 3, Color = "White", Model = "204" }
                }
            };

            return offers;
        }
    }
}
