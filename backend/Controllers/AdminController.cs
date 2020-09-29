using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CorPool.BackEnd.DatabaseModels;
using Microsoft.AspNetCore.Mvc;

namespace CorPool.BackEnd.Controllers {
    public class AdminController : AbstractApiController {
        public AdminController(DatabaseContext database) : base(database) { }

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
        }
    }
}
