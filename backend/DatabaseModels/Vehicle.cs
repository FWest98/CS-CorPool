using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace CorPool.BackEnd.DatabaseModels {
    public class Vehicle {
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }

        [BsonRequired]
        public int Capacity { get; set; } // including driver
    }
}
