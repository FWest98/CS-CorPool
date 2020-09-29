using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CorPool.BackEnd.ApiModels {
    public class Offer {
        public string Id { get; set; }
        public Vehicle Vehicle { get; set; }
        public Location From { get; set; }
        public Location To { get; set; }
        public ApiModels.User User { get; set; }
        public IEnumerable<Confirmation> Confirmations { get; set; }
        public DateTime ArrivalTime { get; set; }
    }
}
