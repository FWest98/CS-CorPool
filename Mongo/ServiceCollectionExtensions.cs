using CorPool.Mongo.DatabaseModels;
using CorPool.Mongo.Options;
using CorPool.Mongo.Providers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CorPool.Mongo {
    public static class ServiceCollectionExtensions {
        public static IServiceCollection AddMongo(this IServiceCollection services, IConfiguration configuration) {
            services.Configure<MongoOptions>(configuration, s => s.BindNonPublicProperties = true);

            services.AddSingleton<MongoDbProvider>();
            services.AddSingleton<DatabaseContext>();

            return services;
        }
    }
}
