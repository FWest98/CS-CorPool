using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Corpool.AspNetCoreTenant;
using CorPool.Mongo.DatabaseModels;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace CorPool.Mongo.Helpers {
    public class TenantResolver : ITenantResolver<Tenant> {
        private readonly DatabaseContext _database;
        public TenantResolver(DatabaseContext database) {
            _database = database;
        }

        public async Task<Tenant> ResolveTentantAsync(string identifier) {
            return await _database.Tenants.AsQueryable().FirstOrDefaultAsync(s => s.Identifier == identifier);
        }
    }
}
