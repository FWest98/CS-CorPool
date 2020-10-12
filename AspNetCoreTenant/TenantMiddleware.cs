using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;

namespace Corpool.AspNetCoreTenant {
    public class TenantMiddleware<TTenant> where TTenant : class, ITenant {
        public const string ContextKey = "Tenant";

        private readonly RequestDelegate _next;
        public TenantMiddleware(RequestDelegate next) => _next = next;

        public async Task Invoke(HttpContext context, ITenantResolver<TTenant> resolver) {
            if (!context.Items.ContainsKey(ContextKey)) {
                // Find tenant identifier
                var identifier = context.Request.Host.Host.Split('.')[0];
                var tenant = await resolver.ResolveTentantAsync(identifier);
                context.Items.Add(ContextKey, tenant);

                // Check if endpoint has tenanted attribute and cancel request if so
                // Similar to what AuthZMiddleware does, bypassing the attribute filters :(
                var endpoint = context.Features.Get<IEndpointFeature>()?.Endpoint;
                if (endpoint?.Metadata.GetMetadata<TenantedAttribute>() != null && tenant == null) {
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsync("No valid tenant specified");

                    return;
                }
            }

            if (_next != null) await _next(context);
        }
    }

    public static class HttpContextExtensions {
        public static TTenant GetTenant<TTenant>(this HttpContext context) where TTenant : class, ITenant {
            return context.Items[TenantMiddleware<ITenant>.ContextKey] as TTenant;
        }
    }
}
