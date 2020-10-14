using Corpool.AspNetCoreTenant;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CorPool.Mongo.DatabaseModels {
    public class Tenant : ITenant {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonRequired]
        public string Identifier { get; set; }

        public string Name { get; set; }
    }

    // Interface for tenanted models
    public interface ITenanted {
        public string TenantId { get; set; }
    }
}
