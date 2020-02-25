namespace HealthChecks
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    public class Program
    {
        private static readonly Dictionary<string, Type> scenarios;

        static Program()
        {
            scenarios = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase)
            {
                { "basic", typeof(BasicStartup) },
                { "dbcontext", typeof(DbContextHealthStartup) }
            };
        }

        public static void Main(string[] args) => BuildHost(args).Run();

        public static IHost BuildHost(string[] args)
        {
            IConfigurationRoot config1 = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables(prefix: "ASPNETCORE_")
                .AddCommandLine(args)
                .Build();

            string scenario = config1["scenario"] ?? "basic";

            if (!scenarios.TryGetValue(scenario, out Type startupType))
            {
                startupType = typeof(BasicStartup);
            }

            return new HostBuilder()
                .ConfigureAppConfiguration(config =>
                {
                    config.AddConfiguration(config1);
                })
                .ConfigureLogging(builder =>
                {
                    builder.SetMinimumLevel(LogLevel.Trace);
                    builder.AddConfiguration(config1);
                    builder.AddConsole();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseKestrel();
                    webBuilder.UseStartup(startupType);
                })
                .Build();
        }
    }
}
