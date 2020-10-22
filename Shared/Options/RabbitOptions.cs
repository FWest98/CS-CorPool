using System;
using System.Collections.Generic;
using System.Text;

namespace CorPool.Shared.Options {
    public class RabbitOptions {
        public string Name { get; set; }
        public string RoutingKey { get; set; } = "routing";
    }
}
