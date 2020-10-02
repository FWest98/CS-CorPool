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

        public async Task Get() {
            await database.Tenants.InsertManyAsync(new List<Tenant> {
                new Tenant {
                    Identifier = "test",
                    Name = "Test Company"
                },
                new Tenant {
                    Identifier = "second",
                    Name = "Second Company"
                }
            });

            var tenants = await database.Tenants.AsQueryable().ToListAsync();

            var users = new List<User> {
                new User {
                    TenantId = tenants.FirstOrDefault(s => s.Identifier == "test")?.Id,
                    UserName = "floris",
                    NormalizedUserName = "FLORIS",
                    Email = "test@test.com"
                },

                new User {
                    TenantId = tenants.FirstOrDefault(s => s.Identifier == "second")?.Id,
                    UserName = "sjouke",
                    NormalizedUserName = "SJOUKE",
                    Email = "test@test.com"
                }
            };

            await database.Users.InsertManyAsync(users);

            users.ForEach(s => {
                _userManager.AddPasswordAsync(s, "Passw0rd!");
                _userManager.SetEmailAsync(s, s.Email);
            });
        }
    }
}
