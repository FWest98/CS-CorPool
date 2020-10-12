using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client.Core.DependencyInjection;

namespace Worker {
    public class Program {
        public static void Main(string[] args) {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) => {
                    // Reset current configuration sources
                    config.Sources.Clear();

                    // Determine current environment
                    var env = context.HostingEnvironment;
                    var subEnv = Environment.GetEnvironmentVariable("SUB_ENV");

                    // Add appsettings files
                    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{env.EnvironmentName}.{subEnv}.json", optional: true, reloadOnChange: true);

                    // Add environments variables as overrides
                    config.AddEnvironmentVariables();

                    // Add command line args as overrides
                    if (args != null) config.AddCommandLine(args);
                })
                .ConfigureServices((hostContext, services) => {
                    // Configure services

                    // Add hosted service
                    services.AddHostedService<Worker>();

                    // Add RabbitMq Client
                    services.AddRabbitMqClient(hostContext.Configuration.GetSection("RabbitMq"))
                        .AddConsumptionExchange("test",
                            hostContext.Configuration.GetSection("RabbitMq").GetSection("Exchange"))
                        .AddAsyncMessageHandlerSingleton<MessageHandler>("routing");

                    // Add health check
                    services.AddHealthChecks();
                    services.Configure<HealthCheckPublisherOptions>(options => {
                        options.Period = TimeSpan.FromSeconds(5);
                    });
                    services.Configure<HealthCheckPublisher.HealthCheckOptions>(hostContext.Configuration.GetSection("HealthCheck"));
                    services.AddSingleton<IHealthCheckPublisher, HealthCheckPublisher>();
                });
    }
}
