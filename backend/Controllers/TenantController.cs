using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CorPool.BackEnd.Controllers;
using CorPool.BackEnd.Models;
using CorPool.BackEnd.Providers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace CarPool.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TenantController : AbstractApiController {
        public TenantController(DatabaseContext database) : base(database) { }

        public async Task<Tenant> Get() => Tenant;
    }
}
