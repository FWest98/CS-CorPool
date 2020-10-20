using StackExchange.Redis;

namespace CorPool.Shared.Options {
    public class RedisOptions {
        public string HostName { get; set; }
        public int Port { get; set; }
        public string Password { get; set; }
        public string ServiceName { get; set; } = null;

        public ConfigurationOptions GetConfiguration() {
            var config = new ConfigurationOptions {
                Password = Password,
                EndPoints = {{HostName, Port}},
                ChannelPrefix = "CorPool"
            };

            // If we are connecting to a sentinel
            if (!string.IsNullOrWhiteSpace(ServiceName))
                config.ServiceName = ServiceName;

            return config;
        }
    }
}
