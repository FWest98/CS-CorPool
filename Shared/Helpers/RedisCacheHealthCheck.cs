using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CorPool.Shared.Helpers {
    public class RedisCacheHealthCheck : IHealthCheck {
        private readonly IDistributedCache _redisCache;
        private static string HealthCheckKey = "HEALTH_PING";

        public RedisCacheHealthCheck(IDistributedCache redisCache) {
            _redisCache = redisCache;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken()) {
            // Try store cache result and catch errors
            try {
                await _redisCache.SetStringAsync(HealthCheckKey, "ping", cancellationToken);
            } catch (Exception e) {
                return new HealthCheckResult(context.Registration.FailureStatus, exception: e);
            }

            return HealthCheckResult.Healthy();
        }
    }
}
