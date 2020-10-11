using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CorPool.BackEnd.Controllers {
    [ApiController]
    [Route("/health")]
    public class HealthController : ControllerBase {
        public string Get() {
            return "OK";
        }
    }
}
