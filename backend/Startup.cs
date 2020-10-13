using System;
using System.Linq;
using Corpool.AspNetCoreTenant;
using CorPool.BackEnd.Helpers;
using CorPool.BackEnd.Helpers.Jwt;
using CorPool.BackEnd.Options;
using CorPool.Mongo;
using CorPool.Mongo.DatabaseModels;
using CorPool.Mongo.Helpers;
using CorPool.Mongo.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client.Core.DependencyInjection;
using StackExchange.Redis;

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
            services.Configure<AuthenticationOptions>(Configuration.GetSection("Authentication"));
            services.Configure<RedisOptions>(Configuration.GetSection("Redis"));

            // Register MVC parts
            services.AddControllers();
            services.AddHttpContextAccessor();
            services.AddHealthChecks();

            // Register Auth
            services.AddIdentityCore<User>()
                .AddUserManager<JwtUserManager>()
                .AddUserStore<UserStore>()
                .AddDefaultTokenProviders();

            var authOptions = new AuthenticationOptions();
            Configuration.GetSection("Authentication").Bind(authOptions);

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => {
                    options.TokenValidationParameters.IssuerSigningKey = 
                        new JwtSigningKey(authOptions.SigningKey);
                    options.TokenValidationParameters.ValidIssuer = authOptions.Authority;
                    options.TokenValidationParameters.ValidAudience = authOptions.Audience;
                    options.RequireHttpsMetadata = false;
                });

            services.AddAuthorization(options => {
                options.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .AddRequirements(new TenantAuthorizationRequirement())
                    .Build();
            });

            // Register tenanting
            services.AddTenanted<Tenant, TenantResolver>();
            services.AddTenantAuth<AuthenticationOptions>();

            // Register other
            services.AddMongo();

            services.AddRabbitMqClient(Configuration.GetSection("RabbitMq"))
                .AddProductionExchange("test", Configuration.GetSection("RabbitMq").GetSection("Exchange"));


            var redisOptions = new RedisOptions();
            Configuration.GetSection("Redis").Bind(redisOptions);

            services.AddStackExchangeRedisCache(options => {
                options.ConfigurationOptions = new ConfigurationOptions {
                    Password = redisOptions.Password,
                    ServiceName = redisOptions.ServiceName,
                    EndPoints = { { redisOptions.HostName, redisOptions.Port } }
                };
            });

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
            app.UseRouting();

            // Custom Tenant middleware
            app.UseTenanted<Tenant>();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors();

            app.UseEndpoints(endpoints => {
                endpoints.MapHealthChecks("/health");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });
        }
    }
}
