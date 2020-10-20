using System;
using System.Linq;
using System.Threading.Tasks;
using Corpool.AspNetCoreTenant;
using CorPool.BackEnd.Helpers;
using CorPool.BackEnd.Helpers.Jwt;
using CorPool.BackEnd.Options;
using CorPool.Mongo;
using CorPool.Mongo.DatabaseModels;
using CorPool.Mongo.Helpers;
using CorPool.Mongo.Options;
using CorPool.Shared;
using CorPool.Shared.Hubs;
using CorPool.Shared.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client.Core.DependencyInjection;
using RabbitMQ.Client.Core.DependencyInjection.Services;
using StackExchange.Redis;

namespace CorPool.BackEnd
{
    public class Startup
    {
        private const string RidesHubUrl = "/api/ride/find";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            // Shared services setup
            services.ConfigureSharedServices(Configuration);

            // Register tenanting
            services.AddTenanted<Tenant, TenantResolver>();
            services.AddTenantAuth<AuthenticationOptions>();

            // Register web parts
            services.AddControllers();
            services.AddHttpContextAccessor();
            services.AddReverseProxy();

            // Register Auth
            services.Configure<AuthenticationOptions>(Configuration.GetSection("Authentication"));

            services.AddIdentityCore<User>()
                .AddUserManager<JwtUserManager>()
                .AddUserStore<UserStore>()
                .AddDefaultTokenProviders();

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer();

            // Add JWT configuration
            services.AddOptions<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme).PostConfigure<IOptions<AuthenticationOptions>>(
                (options, authOptions) => {
                    options.TokenValidationParameters.IssuerSigningKey =
                        new JwtSigningKey(authOptions.Value.SigningKey);
                    options.TokenValidationParameters.ValidIssuer = authOptions.Value.Authority;
                    options.TokenValidationParameters.ValidAudience = authOptions.Value.Audience;
                    options.RequireHttpsMetadata = false;

                    // Add WebSocket auth
                    options.Events = new JwtBearerEvents();
                    options.Events.OnMessageReceived += context => {
                        var token = context.Request.Query["access_token"];

                        // Check if request to hub
                        if (!string.IsNullOrWhiteSpace(token) &&
                            context.HttpContext.Request.Path.StartsWithSegments(RidesHubUrl))
                            context.Token = token;

                        return Task.CompletedTask;
                    };
                });

            services.AddAuthorization(options => {
                options.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .AddRequirements(new TenantAuthorizationRequirement())
                    .Build();
            });

            // Register other
            services.AddRabbitMqProducer(Configuration.GetSection("RabbitMq"));
            services.AddLazyLoading();
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
                endpoints.MapHub<RideHub>(RidesHubUrl);
                endpoints.MapHealthChecks("/health");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });
        }
    }
}
