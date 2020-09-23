using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CorPool.BackEnd.Models;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace CorPool.BackEnd.Helpers {
    public class TenantMiddleware {
        public const string ContextKey = "Tenant";

        private readonly RequestDelegate _next;
        public TenantMiddleware(RequestDelegate next) => _next = next;

        public async Task Invoke(HttpContext context, DatabaseContext database) {
            if (!context.Items.ContainsKey(ContextKey)) {
                // Find tentant identifier
                var identifier = context.Request.Host.Host.Split('.')[0];
                var tenant = await database.Tenants.AsQueryable().FirstOrDefaultAsync(s => s.Identifier == identifier);
                context.Items.Add(ContextKey, tenant);
            }

            if (_next != null) await _next(context);
        }
    }

    public static class HttpContextExtensions {
        public static Tenant GetTenant(this HttpContext context) {
            return context.Items[TenantMiddleware.ContextKey] as Tenant;
        }
    }
}
