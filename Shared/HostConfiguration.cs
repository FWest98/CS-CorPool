using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace CorPool.Shared {
    public static class HostConfiguration {
        public static Action<HostBuilderContext, IConfigurationBuilder> Host<T>(string[] args) where T : class {
            return (context, config) => {
                Configure<T>(context.HostingEnvironment, config, args);
            };
        }

        public static Action<WebHostBuilderContext, IConfigurationBuilder> WebHost<T>(string[] args) where T : class {
            return (context, config) => {
                Configure<T>(context.HostingEnvironment, config, args);
            };
        }

        private static void Configure<T>(IHostEnvironment env, IConfigurationBuilder config, string[] args) where T : class {
            // Reset current configuration sources
            config.Sources.Clear();

            // Determine current environment
            var subEnv = Environment.GetEnvironmentVariable("SUB_ENV");

            // Add appsettings files
            config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.{subEnv}.json", optional: true,
                    reloadOnChange: true);

            // Add user secrets as possible overrides
            if (env.IsDevelopment()) {
                config.AddUserSecrets<T>(optional: true, reloadOnChange: true);
            }

            // Add environments variables as overrides
            config.AddEnvironmentVariables();

            // Add command line args as overrides
            if (args != null) config.AddCommandLine(args);
        }
    }
}
