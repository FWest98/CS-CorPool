using MongoDB.Bson.Serialization.Attributes;

namespace CorPool.Mongo.DatabaseModels {
    public class Location {
        [BsonRequired]
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
