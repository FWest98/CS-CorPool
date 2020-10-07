using MongoDB.Bson.Serialization.Attributes;

namespace CorPool.Mongo.DatabaseModels {
    public class Confirmation {
        [BsonRequired]
        public string UserId { get; set; }

        [BsonRequired]
        public Location PickupPoint { get; set; }
    }
}
