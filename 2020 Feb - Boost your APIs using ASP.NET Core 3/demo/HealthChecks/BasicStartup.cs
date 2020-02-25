namespace HealthChecks
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;
    //using System.Linq;
    //using System.Threading;
    //using System.Threading.Tasks;
    //using Microsoft.AspNetCore.Diagnostics.HealthChecks;
    //using Microsoft.Extensions.Diagnostics.HealthChecks;
    //using Newtonsoft.Json;
    //using Newtonsoft.Json.Linq;

    // Use the `--scenario basic` switch to run this version of the sample.
    public class BasicStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddSingleton<ExampleHealthCheck>();

            services.AddHealthChecks();
            //.AddCheck<ExampleHealthCheck>("example_health_check");
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/health");

                //endpoints.MapHealthChecks("/health", new HealthCheckOptions()
                //{
                //    ResponseWriter = WriteResponse
                //});

                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync(
                        "Navigate to /health to see the health status.");
                });
            });
        }

        //private static Task WriteResponse(HttpContext context, HealthReport result)
        //{
        //    context.Response.ContentType = "application/json";

        //    var json = new JObject(
        //        new JProperty("status", result.Status.ToString()),
        //        new JProperty("results", new JObject(result.Entries.Select(pair =>
        //            new JProperty(pair.Key, new JObject(
        //                new JProperty("status", pair.Value.Status.ToString()),
        //                new JProperty("description", pair.Value.Description),
        //                new JProperty("data", new JObject(pair.Value.Data.Select(
        //                    p => new JProperty(p.Key, p.Value))))))))));

        //    return context.Response.WriteAsync(
        //        json.ToString(Formatting.Indented));
        //}
    }

    //public class ExampleHealthCheck : IHealthCheck
    //{
    //    public Task<HealthCheckResult> CheckHealthAsync(
    //        HealthCheckContext context,
    //        CancellationToken cancellationToken = default)
    //    {
    //        bool healthCheckResultHealthy = false;

    //        if (healthCheckResultHealthy)
    //        {
    //            return Task.FromResult(
    //                HealthCheckResult.Healthy("A healthy result."));
    //        }

    //        return Task.FromResult(
    //            HealthCheckResult.Unhealthy("An unhealthy result."));
    //    }
    //}
}
