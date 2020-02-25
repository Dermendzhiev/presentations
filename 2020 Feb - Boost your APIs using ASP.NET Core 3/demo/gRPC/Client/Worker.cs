namespace Client
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Grpc.Net.Client;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Server;

    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> logger;

        public Worker(ILogger<Worker> logger) => this.logger = logger;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                this.logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                var channel = GrpcChannel.ForAddress("https://localhost:5001/");
                var client = new Greeter.GreeterClient(channel);

                var request = new HelloRequest
                {
                    Name = "Nikolay"
                };

                HelloReply response = await client.SayHelloAsync(request);

                this.logger.LogInformation(response.Message);

                await Task.Delay(2000, stoppingToken);
            }
        }
    }
}
