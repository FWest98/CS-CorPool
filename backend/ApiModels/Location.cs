using DatabaseModels = CorPool.Mongo.DatabaseModels;

namespace CorPool.BackEnd.ApiModels {
    public class Location {
        public string Title { get; set; }
        public string Description { get; set; }

        public Location() { }

        public Location(DatabaseModels.Location location) {
            Title = location.Title;
            Description = location.Description;
        }
    }
}
