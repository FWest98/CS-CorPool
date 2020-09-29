using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace CarPool
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
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

                    // Add user secrets as possible overrides
                    if (env.IsDevelopment()) {
                        config.AddUserSecrets<Startup>(optional: true, reloadOnChange: true);
                    }

                    // Add environments variables as overrides
                    config.AddEnvironmentVariables();

                    // Add command line args as overrides
                    if (args != null) config.AddCommandLine(args);
                })
                .UseStartup<Startup>();
    }
}
