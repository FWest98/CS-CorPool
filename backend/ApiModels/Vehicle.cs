using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CorPool.BackEnd.ApiModels {
    public class Vehicle {
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }
        public int Capacity { get; set; } // including driver
    }
}
