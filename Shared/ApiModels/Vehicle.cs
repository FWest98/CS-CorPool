using System.ComponentModel.DataAnnotations;
using DatabaseModels = CorPool.Mongo.DatabaseModels;

namespace CorPool.Shared.ApiModels {
    public class Vehicle {
        [Required]
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }

        [Required]
        public int Capacity { get; set; } // including driver

        public Vehicle() { }

        public Vehicle(DatabaseModels.Vehicle vehicle) {
            Brand = vehicle.Brand;
            Model = vehicle.Model;
            Color = vehicle.Color;
            Capacity = vehicle.Capacity;
        }
    }
}
