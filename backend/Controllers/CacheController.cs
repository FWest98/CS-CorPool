using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CorPool.BackEnd.Helpers.Jwt;
using CorPool.Mongo.DatabaseModels;

namespace CorPool.BackEnd.Controllers {
    public class CacheController : AbstractApiController {
        public CacheController(Lazy<DatabaseContext> database, Lazy<JwtUserManager> userManager, Lazy<IDistributedCache> cache) : base(database, userManager, cache) { }


        public async Task<string> Get() {
            var now = await Cache.GetStringAsync("time");
            if (now == null) { // expired
                now = DateTime.Now.ToString();
                await Cache.SetStringAsync("time", now, new DistributedCacheEntryOptions {
                    SlidingExpiration = TimeSpan.FromSeconds(10)
                });

                now += " - reset cache";
            } else {
                now += " - from cache";
            }

            return now;
        }
    }
}
