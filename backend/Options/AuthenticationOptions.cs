using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CorPool.BackEnd.Options {
    public class AuthenticationOptions {
        public string Audience { get; set; }
        public string Authority { get; set; }
        public string SigningKey { get; set; }
    }
}
