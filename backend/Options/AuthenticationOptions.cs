using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Corpool.AspNetCoreTenant;

namespace CorPool.BackEnd.Options {
    public class AuthenticationOptions : ITenantAuthOptions {
        public string Audience { get; set; }
        public string Authority { get; set; }
        public string SigningKey { get; set; }
    }
}
