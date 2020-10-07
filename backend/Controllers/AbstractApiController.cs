using Corpool.AspNetCoreTenant;
using CorPool.Mongo.DatabaseModels;
using Microsoft.AspNetCore.Mvc;

namespace CorPool.BackEnd.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public abstract class AbstractApiController : ControllerBase {
        protected readonly DatabaseContext database;
        protected Tenant Tenant => HttpContext.GetTenant<Tenant>();

        protected AbstractApiController(DatabaseContext database) {
            this.database = database;
        }
    }
}
