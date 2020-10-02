using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace CorPool.BackEnd.DatabaseModels {
    public class Confirmation {
        [BsonRequired]
        public string UserId { get; set; }

        [BsonRequired]
        public Location PickupPoint { get; set; }
    }
}
