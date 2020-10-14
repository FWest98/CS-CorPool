using CorPool.Mongo.DatabaseModels;
using CorPool.Mongo.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace CorPool.Mongo {
    public static class ServiceCollectionExtensions {
        public static IServiceCollection AddMongo(this IServiceCollection services) {
            services.AddSingleton<MongoDbProvider>();
            services.AddSingleton<DatabaseContext>();

            return services;
        }
    }
}
