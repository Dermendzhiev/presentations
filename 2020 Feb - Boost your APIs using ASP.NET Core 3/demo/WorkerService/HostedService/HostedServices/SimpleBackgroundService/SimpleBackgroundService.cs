namespace HostedService.HostedServices.SimpleBackgroundService
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    public class SimpleBackgroundService : BackgroundService
    {
        private readonly ILogger<SimpleBackgroundService> logger;

        public SimpleBackgroundService(ILogger<SimpleBackgroundService> logger) => this.logger = logger;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                this.logger.LogInformation($"{nameof(SimpleBackgroundService)} is running.");

                // Background tasks (aggregation, file streaming, etc.)

                await Task.Delay(1000);
            }
        }
    }
}
