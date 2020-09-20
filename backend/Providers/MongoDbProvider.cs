using CorPool.BackEnd.Options;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CorPool.BackEnd.Providers {
    public class MongoDbProvider {
        private readonly IOptionsMonitor<MongoOptions> _optionsMonitor;
        private MongoClient _client;

        public MongoDbProvider(IOptionsMonitor<MongoOptions> optionsMonitor) {
            _optionsMonitor = optionsMonitor;
            _optionsMonitor.OnChange(s => {
                _client = null;
            });
        }

        public bool IsAvailable() => _client != null;

        public MongoClient GetClient() => 
            _client ??= new MongoClient(_optionsMonitor.CurrentValue.GetSettings());

        public IMongoDatabase GetDatabase() => GetClient().GetDatabase(_optionsMonitor.CurrentValue.DatabaseName);

        public IMongoCollection<T> GetCollection<T>(string name) => GetDatabase().GetCollection<T>(name);
    }
}
