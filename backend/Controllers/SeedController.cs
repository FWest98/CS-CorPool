using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CorPool.BackEnd.Helpers.Jwt;
using CorPool.Mongo.DatabaseModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using MongoDB.Driver;

namespace CorPool.BackEnd.Controllers {
    public class SeedController : AbstractApiController {
        public SeedController(Lazy<DatabaseContext> Database, Lazy<JwtUserManager> userManager, Lazy<IDistributedCache> cache) : base(Database, userManager, cache) { }

        public async Task<ActionResult<string>> Get() {
            // Clear Database
            await Database.Offers.DeleteManyAsync(s => true);
            await Database.Users.DeleteManyAsync(s => true);
            await Database.Tenants.DeleteManyAsync(s => true);

            // Insert tenants
            var shell = new Tenant {
                Identifier = "shell",
                Name = "Shell B.V."
            };
            var ah = new Tenant {
                Identifier = "ah",
                Name = "Albert Heijn"
            };

            var tenants = new List<Tenant> { shell, ah };
            await Database.Tenants.InsertManyAsync(tenants);

            // Insert users
            var volvo = new Vehicle { Brand = "Volvo", Model = "V70", Color = "Blue", Capacity = 5 };
            var tesla = new Vehicle { Brand = "Tesla", Model = "Model S", Color = "Black", Capacity = 5 };
            var golf = new Vehicle { Brand = "Volkswagen", Model = "Golf", Color = "Red", Capacity = 2 };
            var touran = new Vehicle { Brand = "Volkswagen", Model = "Touran", Color = "Gray", Capacity = 9 };

            var floris = new User {
                TenantId = shell.Id,
                UserName = "floris",
                NormalizedUserName = "FLORIS",
                Email = "floris@shell.com",
                FullName = "Floris Westerman",
                Vehicles = new List<Vehicle> { volvo, tesla }
            };

            var sjouke = new User {
                TenantId = ah.Id,
                UserName = "sjouke",
                NormalizedUserName = "SJOUKE",
                Email = "sjouke@ah.nl",
                FullName = "Sjouke de Vries",
                Vehicles = new List<Vehicle> { golf, touran }
            };

            var alexander = new User {
                TenantId = shell.Id,
                UserName = "alexander",
                NormalizedUserName = "ALEXANDER",
                Email = "alexander@shell.com",
                FullName = "Alexander Lazovik"
            };

            var vasilios = new User {
                TenantId = ah.Id,
                UserName = "vasilios",
                NormalizedUserName = "VASILIOS",
                Email = "vasilios@ah.nl",
                FullName = "Vasilios Andrikopoulos"
            };

            var users = new List<User> { floris, sjouke, alexander, vasilios };
            await Database.Users.InsertManyAsync(users);

            // Set users passwords and emails
            users.ForEach(s => {
                UserManager.AddPasswordAsync(s, "Passw0rd!");
                UserManager.SetEmailAsync(s, s.Email);
            });

            // Insert offers
            var offers = new List<Offer> {
                new Offer {
                    TenantId = shell.Id,
                    UserId = floris.Id,
                    Vehicle = volvo,
                    From = new Location { Title = "Home", Description = "Stadspark, Groningen" },
                    To = new Location { Title = "Shell Office", Description = "Schiphol, Amsterdam" },
                    ArrivalTime = new DateTime(2020, 11, 7, 14, 30, 00),
                    Confirmations = new List<Confirmation> {
                        new Confirmation {
                            PickupPoint = new Location { Title = "Home", Description = "Dorpsweg 41, Haren" },
                            UserId = alexander.Id
                        }
                    }
                },
                new Offer {
                    TenantId = ah.Id,
                    UserId = sjouke.Id,
                    Vehicle = touran,
                    From = new Location { Title = "Zernike", Description = "Bernoulliborg, Zernike, Groningen" },
                    To = new Location { Title = "Albert Heijn Utrecht", Description = "Albert Heijn, Utrecht" }
                }
            };

            await Database.Offers.InsertManyAsync(offers);

            return "Seeding Database succeeded";
        }
    }
}
