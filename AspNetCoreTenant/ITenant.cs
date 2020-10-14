using System;
using System.Collections.Generic;
using System.Text;

namespace Corpool.AspNetCoreTenant {
    public interface ITenant {
        public string Id { get; set; }
    }
}
