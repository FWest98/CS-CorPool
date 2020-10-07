using System;
using System.Collections.Generic;
using System.Text;

namespace Corpool.AspNetCoreTenant {
    public interface ITenantAuthOptions {
        public string Authority { get; set; }
    }
}
