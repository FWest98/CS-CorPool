using MongoDB.Bson.Serialization.Attributes;

namespace CorPool.Mongo.DatabaseModels {
    public class Vehicle {
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }

        [BsonRequired]
        public int Capacity { get; set; } // including driver
    }
}
