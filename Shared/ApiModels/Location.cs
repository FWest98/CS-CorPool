using System.ComponentModel.DataAnnotations;
using DatabaseModels = CorPool.Mongo.DatabaseModels;

namespace CorPool.Shared.ApiModels {
    public class Location {
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }

        public Location() { }

        public Location(DatabaseModels.Location location) {
            Title = location.Title;
            Description = location.Description;
        }
    }
}
