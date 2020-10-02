using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CorPool.BackEnd.DatabaseModels;
using CorPool.BackEnd.Helpers.Jwt;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace CorPool.BackEnd.Controllers {
    public class SeedController : AbstractApiController {
        private readonly JwtUserManager _userManager;

        public SeedController(DatabaseContext database, JwtUserManager userManager) : base(database) {
            _userManager = userManager;
        }

        public async Task<ActionResult<string>> Get() {
            // Clear database
            await database.Offers.DeleteManyAsync(s => true);
            await database.Users.DeleteManyAsync(s => true);
            await database.Tenants.DeleteManyAsync(s => true);

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
            await database.Tenants.InsertManyAsync(tenants);

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
            await database.Users.InsertManyAsync(users);

            // Set users passwords and emails
            users.ForEach(s => {
                _userManager.AddPasswordAsync(s, "Passw0rd!");
                _userManager.SetEmailAsync(s, s.Email);
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

            await database.Offers.InsertManyAsync(offers);

            return "Seeding database succeeded";
        }
    }
}
