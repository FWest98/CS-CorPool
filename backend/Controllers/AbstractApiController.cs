using System;
using System.Threading.Tasks;
using Corpool.AspNetCoreTenant;
using CorPool.BackEnd.Helpers.Jwt;
using CorPool.Mongo.DatabaseModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace CorPool.BackEnd.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public abstract class AbstractApiController : ControllerBase {
        private readonly Lazy<DatabaseContext> _database;
        protected DatabaseContext Database => _database.Value;

        protected Tenant Tenant => HttpContext.GetTenant<Tenant>();

        private readonly Lazy<JwtUserManager> _userManager;
        protected JwtUserManager UserManager => _userManager.Value;

        protected new Task<User> User => UserManager.GetUserAsync(base.User);

        private readonly Lazy<IDistributedCache> _cache;
        protected IDistributedCache Cache => _cache.Value;

        protected AbstractApiController(Lazy<DatabaseContext> database, Lazy<JwtUserManager> userManager, Lazy<IDistributedCache> cache) {
            _database = database;
            _userManager = userManager;
            _cache = cache;
        }
    }
}
