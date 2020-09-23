using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CorPool.BackEnd.ApiModels {
    public class Confirmation {
        public string Id { get; set; }
        public Offer Offer { get; set; }
        public User User { get; set; }
        public Location PickupPoint { get; set; }
    }
}
