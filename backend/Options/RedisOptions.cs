using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CorPool.BackEnd.Options {
    public class RedisOptions {
        public string HostName { get; set; }
        public int Port { get; set; }
        public string Password { get; set; }
    }
}
