using System;
using System.Collections.Generic;
using System.Text;

namespace CorPool.Shared.ApiModels {
    public class RideRequest {
        public Location From { get; set; }
        public Location To { get; set; }
        public DateTime ArrivalTime { get; set; }
    }
}
