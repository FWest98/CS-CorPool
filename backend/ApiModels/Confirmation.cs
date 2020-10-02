using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CorPool.BackEnd.ApiModels {
    public class Confirmation {
        public User User { get; set; }
        public Location PickupPoint { get; set; }

        public Confirmation() {}
        public Confirmation(DatabaseModels.Confirmation confirmation) {
            PickupPoint = new Location(confirmation.PickupPoint);
            User = new User { Id = confirmation.UserId };
        }
    }
}
