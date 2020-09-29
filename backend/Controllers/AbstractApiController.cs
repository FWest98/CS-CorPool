using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CorPool.BackEnd.Helpers;
using CorPool.BackEnd.DatabaseModels;
using Microsoft.AspNetCore.Mvc;

namespace CorPool.BackEnd.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public abstract class AbstractApiController : ControllerBase {
        protected readonly DatabaseContext database;
        protected Tenant Tenant => HttpContext.GetTenant();

        protected AbstractApiController(DatabaseContext database) {
            this.database = database;
        }
    }
}
