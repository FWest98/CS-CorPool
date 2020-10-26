using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatabaseModels = CorPool.Mongo.DatabaseModels;

namespace CorPool.Shared.ApiModels {
    public class Offer {
        public string Id { get; set; }
        public Vehicle Vehicle { get; set; }
        public Location From { get; set; }
        public Location To { get; set; }
        public DateTime ArrivalTime { get; set; }

        public List<Confirmation> Confirmations { get; set; } = new List<Confirmation>();
        public int RemainingCapacity { get; private set; }

        public User User { get; set; }

        public Offer() {}
        public Offer(DatabaseModels.Offer offer) {
            Id = offer.Id;
            Vehicle = new Vehicle(offer.Vehicle);
            From = new Location(offer.From);
            To = new Location(offer.To);
            ArrivalTime = offer.ArrivalTime;
            RemainingCapacity = offer.RemainingCapacity;

            Confirmations = offer.Confirmations.Select(s => new Confirmation(s)).ToList();
            User = new User { Id = offer.UserId };
        }

        public async Task SetUsers(Func<string, Task<User>> getUser) {
            User = await getUser(User.Id);
            await Task.WhenAll(
                Confirmations.Select(async s => s.User = await getUser(s.User.Id))
            );
        }
    }
}
