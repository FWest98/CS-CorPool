using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CorPool.Mongo.DatabaseModels {
    public class Offer : ITenanted {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonRequired]
        public string TenantId { get; set; }

        public Vehicle Vehicle { get; set; }

        [BsonRequired]
        public Location From { get; set; }

        [BsonRequired]
        public Location To { get; set; }
        public DateTime ArrivalTime { get; set; }
        public List<Confirmation> Confirmations { get; set; } = new List<Confirmation>();

        [BsonIgnore]
        public int RemainingCapacity => Vehicle.Capacity - Confirmations.Count - 1; // -1 for driver

        [BsonRequired]
        public string UserId { get; set; }
    }
}
