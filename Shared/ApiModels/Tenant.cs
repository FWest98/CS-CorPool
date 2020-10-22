using DatabaseModels = CorPool.Mongo.DatabaseModels;

namespace CorPool.Shared.ApiModels {
    public class Tenant {
        public string Id { get; set; }
        public string Name { get; set; }

        public Tenant() { }
        public Tenant(DatabaseModels.Tenant tenant) {
            Id = tenant.Id;
            Name = tenant.Name;
        }
    }
}
