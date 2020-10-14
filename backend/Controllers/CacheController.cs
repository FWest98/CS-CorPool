using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CorPool.Mongo.DatabaseModels;

namespace CorPool.BackEnd.Controllers {
    public class CacheController : AbstractApiController {
        private readonly IDistributedCache _cache;
        public CacheController(DatabaseContext database, IDistributedCache cache) : base(database) {
            _cache = cache;
        }

        public async Task<string> Get() {
            var now = await _cache.GetStringAsync("time");
            if (now == null) { // expired
                now = DateTime.Now.ToString();
                await _cache.SetStringAsync("time", now, new DistributedCacheEntryOptions {
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
