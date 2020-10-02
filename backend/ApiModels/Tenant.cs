using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CorPool.BackEnd.ApiModels {
    public class Tenant {
        public string Id { get; set; }
        public string Name { get; set; }

        public Tenant() { }
        public Tenant(DatabaseModels.Tenant tenant) {
            Id = tenant.Id;
            Name = tenant.Name;
        }
    }
}
