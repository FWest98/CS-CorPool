using MongoDB.Driver;

namespace CorPool.BackEnd.Options {
    public class MongoOptions {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { private get; set; }

        public string CredentialsDatabaseName { get; set; } = "admin";
        public string DatabaseName { get; set; }

        public MongoClientSettings GetSettings() {
            return new MongoClientSettings {
                Credential = MongoCredential.CreateCredential(CredentialsDatabaseName, Username, Password),
                Server = new MongoServerAddress(Host, Port)
            };
        }
    }
}
