using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.SignalR;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CorPool.Mongo.DatabaseModels {
    public class RideRequest : ITenanted {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonRequired]
        public string TenantId { get; set; }

        [BsonRequired]
        public string UserId { get; set; }

        public Location From { get; set; }
        public Location To { get; set; }
        public DateTime ArrivalTime { get; set; }
    }
}
