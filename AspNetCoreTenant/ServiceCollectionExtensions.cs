using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Corpool.AspNetCoreTenant {
    public static class ServiceCollectionExtensions {
        public static IServiceCollection AddTenanted<TTenant>(this IServiceCollection services, Func<string, Task<TTenant>> resolveAsync)
            where TTenant : class, ITenant {
            services.AddSingleton<ITenantAccessor, TenantAccessor>();
            services.AddSingleton<ITenantAccessor<TTenant>, TenantAccessor<TTenant>>();
            services.AddSingleton<ITenantResolver<TTenant>>(_ => new TenantResolver<TTenant>(resolveAsync));

            return services;
        }

        public static IServiceCollection AddTenanted<TTenant, TResolver>(this IServiceCollection services)
            where TTenant : class, ITenant where TResolver : class, ITenantResolver<TTenant> {
            services.AddSingleton<ITenantAccessor, TenantAccessor>();
            services.AddSingleton<ITenantAccessor<TTenant>, TenantAccessor<TTenant>>();
            services.AddSingleton<ITenantResolver<TTenant>, TResolver>();

            return services;
        }

        public static IServiceCollection AddTenantAuth<TOptions>(this IServiceCollection services)
            where TOptions : class, ITenantAuthOptions {
            services.AddSingleton<IAuthorizationHandler, TenantAuthorizationHandler<TOptions>>();

            return services;
        }

        public static IApplicationBuilder UseTenanted<TTenant>(this IApplicationBuilder app)
            where TTenant : class, ITenant {
            app.UseMiddleware<TenantMiddleware<TTenant>>();

            return app;
        }
    }
}
