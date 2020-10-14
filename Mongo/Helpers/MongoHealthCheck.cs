using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CorPool.Mongo.Providers;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using MongoDB.Driver;

namespace CorPool.Mongo.Helpers {
    public class MongoHealthCheck : IHealthCheck {
        private readonly MongoDbProvider _provider;
        public MongoHealthCheck(MongoDbProvider provider) {
            _provider = provider;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken()) {
            try {
                // Get collections in a database
                var asyncCursor = await _provider.GetDatabase()
                    .ListCollectionNamesAsync(cancellationToken: cancellationToken);
                var result = await asyncCursor.FirstOrDefaultAsync(cancellationToken);

                return HealthCheckResult.Healthy();
            } catch (Exception e) {
                return new HealthCheckResult(context.Registration.FailureStatus, exception: e);
            }
        }
    }
}
