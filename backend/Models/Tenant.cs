using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CorPool.BackEnd.Models {
    public class Tenant {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Identifier { get; set; }

        public string Name { get; set; }
    }
}
