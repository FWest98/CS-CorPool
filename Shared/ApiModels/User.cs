using DatabaseModels = CorPool.Mongo.DatabaseModels;

namespace CorPool.Shared.ApiModels {
    public class User {
        public string Id { get; set; }
        public string Name { get; set; }

        public User() { }
        public User(DatabaseModels.User user) {
            Id = user.Id;
            Name = user.FullName;
        }
    }
}
