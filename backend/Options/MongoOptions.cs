﻿using MongoDB.Driver;

namespace CorPool.BackEnd.Options {
    public class MongoOptions {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { private get; set; }

        public string ReplicaSetName { get; set; } = null;
        public string CredentialsDatabaseName { get; set; } = "admin";
        public string DatabaseName { get; set; }

        public MongoClientSettings GetSettings() {
            var settings = new MongoClientSettings {
                Credential = MongoCredential.CreateCredential(CredentialsDatabaseName, Username, Password),
                Server = new MongoServerAddress(Host, Port)
            };

            if (!string.IsNullOrWhiteSpace(ReplicaSetName)) {
                settings.ReplicaSetName = ReplicaSetName;
            }

            return settings;
        }
    }
}
