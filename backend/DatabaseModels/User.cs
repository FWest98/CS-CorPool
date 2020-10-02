using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CorPool.BackEnd.DatabaseModels {
    public class User : ITenanted {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string TenantId { get; set; }

        [ProtectedPersonalData]
        public virtual string UserName { get; set; }

        public virtual string NormalizedUserName { get; set; }

        [ProtectedPersonalData]
        public virtual string Email { get; set; }

        public virtual string NormalizedEmail { get; set; }

        [PersonalData]
        public virtual bool EmailConfirmed { get; set; }

        public virtual string PasswordHash { get; set; }

        public virtual string SecurityStamp { get; set; }

        public virtual string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();

        [ProtectedPersonalData]
        public virtual string PhoneNumber { get; set; }

        [PersonalData]
        public virtual bool PhoneNumberConfirmed { get; set; }

        [PersonalData]
        public virtual bool TwoFactorEnabled { get; set; }

        public virtual DateTimeOffset? LockoutEnd { get; set; }

        public virtual bool LockoutEnabled { get; set; }

        public virtual int AccessFailedCount { get; set; }

        public override string ToString() => this.UserName;
    }
}
