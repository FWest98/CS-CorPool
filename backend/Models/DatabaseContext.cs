using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CorPool.BackEnd.Providers;
using MongoDB.Driver;

namespace CorPool.BackEnd.Models {
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
}
