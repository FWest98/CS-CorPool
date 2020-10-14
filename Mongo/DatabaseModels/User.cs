using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CorPool.Mongo.DatabaseModels {
    public class User : ITenanted {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonRequired]
        public string TenantId { get; set; }

        [BsonRequired]
        public string UserName { get; set; }
        public string NormalizedUserName { get; set; }

        public string FullName { get; set; }

        [BsonRequired]
        public string Email { get; set; }
        public string NormalizedEmail { get; set; }
        public virtual bool EmailConfirmed { get; set; }

        public virtual string PasswordHash { get; set; }
        public virtual string SecurityStamp { get; set; }

        public virtual string PhoneNumber { get; set; }
        public virtual bool PhoneNumberConfirmed { get; set; }

        public List<Vehicle> Vehicles { get; set; }

        public override string ToString() => UserName;
    }
}
