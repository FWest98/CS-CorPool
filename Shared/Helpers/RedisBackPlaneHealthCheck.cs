using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CorPool.Shared.Helpers {
    public class RedisBackPlaneHealthCheck : IHealthCheck {
        public bool HasConnection { get; set; } = false;

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken()) {
            return Task.FromResult(
                HasConnection
                    ? HealthCheckResult.Healthy()
                    : HealthCheckResult.Unhealthy("The SignalR backplane does not have a connection to Redis")
            );
        }
    }
}
