using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CorPool.Worker {
    internal class HealthCheckPublisher : IHealthCheckPublisher {
        private readonly HealthCheckOptions _healthCheckOptions;
        private readonly ILogger<HealthCheckPublisher> _logger;

        public HealthCheckPublisher(IOptions<HealthCheckOptions> healthCheckOptions, ILogger<HealthCheckPublisher> logger) {
            _healthCheckOptions = healthCheckOptions.Value;
            _logger = logger;
        }

        // We publish the health check file to the given path
        public async Task PublishAsync(HealthReport report, CancellationToken cancellationToken) {
            if (!_healthCheckOptions.Enabled) return;

            if (report.Status != HealthStatus.Healthy) {
                _logger.LogInformation($"Health Check Result: {report.Status}");
                foreach (var item in report.Entries) {
                    _logger.LogInformation($"Entry {item.Key}: {item.Value.Status} due to {item.Value.Exception}");
                }
            }

            // Write to file when healthy, otherwise delete file
            if(report.Status == HealthStatus.Healthy || report.Status == HealthStatus.Degraded) {
                await File.WriteAllTextAsync(_healthCheckOptions.FilePath, "Healthy", cancellationToken);
            } else {
                // Only delete when existing
                if(File.Exists(_healthCheckOptions.FilePath))
                    File.Delete(_healthCheckOptions.FilePath);
            }
        }

        internal class HealthCheckOptions {
            public bool Enabled { get; set; } = false;
            public string FilePath { get; set; }
        }
    }
}
