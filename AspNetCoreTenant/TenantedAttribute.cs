using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Corpool.AspNetCoreTenant {
    /**
     * Attribute to limit access when no tenant is specified.
     * Handled in the TenantMiddleware
     */
    public class TenantedAttribute : Attribute { }
}
