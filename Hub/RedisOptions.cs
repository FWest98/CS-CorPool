namespace CorPool.Hub {
    public class RedisOptions {
        public string HostName { get; set; }
        public int Port { get; set; }
        public string Password { get; set; }
        public string ServiceName { get; set; } = null;
    }
}
