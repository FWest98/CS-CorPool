using CorPool.BackEnd.Providers;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace CorPool.BackEnd.DatabaseModels {
    /**
     * Class to provide easy access to queryables of data sets
     */
    public class DatabaseContext {
        private MongoDbProvider _dbProvider;
        public DatabaseContext(MongoDbProvider dbProvider) {
            _dbProvider = dbProvider;
        }

        public IMongoCollection<Tenant> Tenants => _dbProvider.GetCollection<Tenant>("Tenants");
    }

    public static class TenantedMongoCollectionExtensions {
        public static IMongoQueryable<T> Tenanted<T>(this IMongoCollection<T> collection, Tenant tenant)
            where T : ITenanted
            => collection.AsQueryable().Where(s => s.TenantId == tenant.Id);
    }
}
