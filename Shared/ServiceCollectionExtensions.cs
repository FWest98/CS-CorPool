using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CorPool.BackEnd.Helpers;
using CorPool.Mongo;
using CorPool.Mongo.Helpers;
using CorPool.Shared.Helpers;
using CorPool.Shared.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.StackExchangeRedis;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using MongoDB.Libmongocrypt;
using RabbitMQ.Client.Core.DependencyInjection;
using RabbitMQ.Client.Core.DependencyInjection.Services;
using StackExchange.Redis;
using RedisOptions = Microsoft.AspNetCore.SignalR.StackExchangeRedis.RedisOptions;

namespace CorPool.Shared {
    public static class ServiceCollectionExtensions {
        public static IServiceCollection AddSignalR(this IServiceCollection services) {
            // Configure main SignalR
            services.AddSignalR(options => {

                })
                .AddJsonProtocol();

            // Add Redis hub lifetime manager
            services.AddSingleton(typeof(HubLifetimeManager<>), typeof(RedisHubLifetimeManager<>));

            // Configure SignalR Redis Options
            services.AddOptions<RedisOptions>()
                .Configure<IOptions<Options.RedisOptions>, RedisBackPlaneHealthCheck>((options, redis, healthCheck) => {
                    options.Configuration = redis.Value.GetConfiguration();
                    options.Configuration.AbortOnConnectFail = false;
                    options.ConnectionFactory = async writer => {
                        var connection = await ConnectionMultiplexer.ConnectAsync(options.Configuration, writer);
                        if(!connection.IsConnected) {
                            await writer.WriteLineAsync("Could not connect to Redis for the SignalR backplane");
                            return connection;
                        }

                        connection.ConnectionFailed += (_, e) => {
                            healthCheck.HasConnection = false;
                        };
                        connection.ConnectionRestored += (_, e) => {
                            healthCheck.HasConnection = true;
                        };

                        healthCheck.HasConnection = true;
                        return connection;
                    };
                });

            // Configure authentication
            services.AddSingleton<IUserIdProvider, DefaultUserIdProvider>();

            return services;
        }

        public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration) {
            // Add Redis options
            services.Configure<Options.RedisOptions>(configuration);

            return services;
        }

        public static IServiceCollection AddRedisCache(this IServiceCollection services) {
            // Add Redis cache
            services.AddSingleton<IDistributedCache, RedisCache>();

            // Configure Redis Cache options
            services.AddOptions<RedisCacheOptions>()
                .Configure<IOptions<Options.RedisOptions>>((options, redis) => {
                    options.ConfigurationOptions = redis.Value.GetConfiguration();
                });

            return services;
        }

        public static IServiceCollection AddRabbitMqProducer(this IServiceCollection services, IConfiguration config) {
            return services.AddRabbitMqProducer(config, config.GetSection("Exchange"));
        }

        public static IServiceCollection AddRabbitMqProducer(this IServiceCollection services, IConfiguration config, IConfiguration exchangeConfig) {
            // Register options
            services.Configure<RabbitOptions>(exchangeConfig);

            // Register RabbitMQ
            services.AddRabbitMqClient(config)
                .AddProductionExchange(exchangeConfig["Name"], exchangeConfig);

            return services;
        }

        public static IServiceCollection AddRabbitMqConsumer(this IServiceCollection services, IConfiguration config) {
            return services.AddRabbitMqConsumer(config, config.GetSection("Exchange"));
        }

        public static IServiceCollection AddRabbitMqConsumer(this IServiceCollection services, IConfiguration config, IConfiguration exchangeConfig) {
            // Register options
            services.Configure<RabbitOptions>(exchangeConfig);

            // Register RabbitMQ
            services.AddRabbitMqClient(config)
                .AddConsumptionExchange(exchangeConfig["Name"], exchangeConfig);

            return services;
        }

        public static IServiceCollection AddLazyLoading(this IServiceCollection services) {
            // Allow lazy loading of types
            foreach (var service in services.ToList()) {
                var lazyType = typeof(Lazy<>).MakeGenericType(service.ServiceType);
                var lazyDepType = typeof(LazyDep<>).MakeGenericType(service.ServiceType);
                services.Add(new ServiceDescriptor(lazyType, lazyDepType, service.Lifetime));
            }

            // Fallback
            services.AddTransient(typeof(Lazy<>), typeof(LazyDep<>));

            return services;
        }

        public static IServiceCollection AddReverseProxy(this IServiceCollection services) {
            // Optionally configure nginx reverse proxy compatibility
            if (Environment.GetEnvironmentVariable("ASPNETCORE_FORWARDEDHEADERS_ENABLED") == "true") {
                services.Configure<ForwardedHeadersOptions>(options => {
                    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;

                    // We clear the whitelist as we have explicitly enabled proxying now
                    options.KnownNetworks.Clear();
                    options.KnownProxies.Clear();
                });
            }

            return services;
        }

        public static IServiceCollection ConfigureSharedServices(this IServiceCollection services, IConfiguration configuration) {
            services.AddRedis(configuration.GetSection("Redis"));
            services.AddRedisCache();
            services.AddMongo(configuration.GetSection("Mongo"));
            
            // Direct call to prevent conflict
            AddSignalR(services);

            // Health checks
            var timeout = TimeSpan.FromSeconds(3);
            services.AddHealthChecks()
                //.AddCheck<RedisCacheHealthCheck>("Redis", HealthStatus.Unhealthy, timeout: timeout)
                .AddCheck<RedisBackPlaneHealthCheck>("RedisBackPlane", HealthStatus.Unhealthy, timeout: timeout)
                .AddCheck<MongoHealthCheck>("Mongo", HealthStatus.Unhealthy, timeout: timeout)
                .AddRabbitMQ(
                    sp => sp.GetRequiredService<IQueueService>().Connection,
                    "RabbitMQ",
                    HealthStatus.Unhealthy,
                    timeout: timeout
                );

            return services;
        }
    }
}
