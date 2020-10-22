using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CorPool.BackEnd.Helpers.Jwt;
using CorPool.Mongo.DatabaseModels;
using Microsoft.Extensions.Caching.Distributed;
using MongoDB.Driver;
using ApiModels = CorPool.Shared.ApiModels;

namespace CorPool.BackEnd.Controllers
{
    public class TenantController : AbstractApiController {
        public TenantController(Lazy<DatabaseContext> database, Lazy<JwtUserManager> userManager, Lazy<IDistributedCache> cache) : base(database, userManager, cache) { }

        public async Task<IEnumerable<ApiModels.Tenant>> Get() {
            // List all tenants
            var tenants = await Database.Tenants.AsQueryable().ToListAsync();
            return tenants.Select(s => new ApiModels.Tenant(s));
        }
    }
}
