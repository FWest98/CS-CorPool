using System;
using System.Linq;
using CorPool.BackEnd.Helpers;
using CorPool.BackEnd.Models;
using CorPool.BackEnd.Options;
using CorPool.BackEnd.Providers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CarPool
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            // Register Options
            services.Configure<MongoOptions>(Configuration.GetSection("Mongo"), o => o.BindNonPublicProperties = true);

            // Register MVC parts
            services.AddControllers();

            // Register providers
            services.AddSingleton<MongoDbProvider>();
            services.AddSingleton<DatabaseContext>();

            // Optionally configure nginx reverse proxy compatibility
            if (Environment.GetEnvironmentVariable("ASPNETCORE_FORWARDEDHEADERS_ENABLED") == "true") {
                services.Configure<ForwardedHeadersOptions>(options => {
                    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;

                    // We clear the whitelist as we have explicitly enabled proxying now
                    options.KnownNetworks.Clear();
                    options.KnownProxies.Clear();
                });
            }

            // Allow lazy-loading
            foreach (var service in services.ToList()) {
                var lazyType = typeof(Lazy<>);
                lazyType = lazyType.MakeGenericType(service.ServiceType);

                var lazyDepType = typeof(LazyDep<>);
                lazyDepType = lazyDepType.MakeGenericType(service.ServiceType);

                services.Add(new ServiceDescriptor(lazyType, lazyDepType, service.Lifetime));
            }

            // Fallback
            services.AddTransient(typeof(Lazy<>), typeof(LazyDep<>));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
                app.UseHttpsRedirection();
            }

            app.UseStaticFiles();

            // Custom Tenant middleware
            app.UseMiddleware<TenantMiddleware>();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });
        }
    }
}
