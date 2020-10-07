using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Corpool.AspNetCoreTenant {
    /**
     * Resource filter to limit access when no tenant is specified.
     */
    public class TenantedAttribute : Attribute, IResourceFilter {
        public void OnResourceExecuted(ResourceExecutedContext context) { }

        public void OnResourceExecuting(ResourceExecutingContext context) {
            // Return error when requesting this controller while not specifying a tenant
            if(context.HttpContext.GetTenant<ITenant>() == null)
                context.Result = new BadRequestObjectResult("No valid tenant specified");
        }
    }
}
