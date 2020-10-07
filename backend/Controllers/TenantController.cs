using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CorPool.Mongo.DatabaseModels;
using MongoDB.Driver;

namespace CorPool.BackEnd.Controllers
{
    public class TenantController : AbstractApiController {
        public TenantController(DatabaseContext database) : base(database) { }

        public async Task<IEnumerable<ApiModels.Tenant>> Get() {
            // List all tenants
            var tenants = await database.Tenants.AsQueryable().ToListAsync();
            return tenants.Select(s => new ApiModels.Tenant(s));
        }
    }
}
