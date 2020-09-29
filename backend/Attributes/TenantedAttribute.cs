using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CorPool.BackEnd.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CorPool.BackEnd.Attributes {
    /**
     * Resource filter to limit access when no tenant is specified.
     */
    public class TenantedAttribute : Attribute, IResourceFilter {
        public void OnResourceExecuted(ResourceExecutedContext context) { }

        public void OnResourceExecuting(ResourceExecutingContext context) {
            // Return error when requesting this controller while not specifying a tenant
            if(context.HttpContext.GetTenant() == null)
                context.Result = new BadRequestObjectResult("No valid tenant specified");
        }
    }
}
