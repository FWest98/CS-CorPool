using DatabaseModels = CorPool.Mongo.DatabaseModels;

namespace CorPool.Shared.ApiModels {
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
