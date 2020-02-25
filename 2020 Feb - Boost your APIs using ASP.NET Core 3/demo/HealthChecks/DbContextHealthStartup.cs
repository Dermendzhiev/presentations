namespace HealthChecks
{
    using System;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using HealthChecks.Data;

    // Use the `--scenario dbcontext` switch to run this version of the sample.
    public class DbContextHealthStartup
    {
        public DbContextHealthStartup(IConfiguration configuration) => this.Configuration = configuration;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHealthChecks()
                .AddDbContextCheck<AppDbContext>();

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(
                    this.Configuration["ConnectionStrings:DefaultConnection"]);
            });
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/health");

                endpoints.MapGet("/createdatabase", async context =>
                {
                    await context.Response.WriteAsync("Creating the database...");
                    await context.Response.WriteAsync(Environment.NewLine);
                    await context.Response.Body.FlushAsync();

                    AppDbContext appDbContext = 
                        context.RequestServices.GetRequiredService<AppDbContext>();
                    await appDbContext.Database.EnsureCreatedAsync();

                    await context.Response.WriteAsync("Done!");
                    await context.Response.WriteAsync(Environment.NewLine);
                    await context.Response.WriteAsync(
                        "Navigate to /health to see the health status.");
                    await context.Response.WriteAsync(Environment.NewLine);
                });

                endpoints.MapGet("/deletedatabase", async context =>
                {
                    await context.Response.WriteAsync("Deleting the database...");
                    await context.Response.WriteAsync(Environment.NewLine);
                    await context.Response.Body.FlushAsync();

                    AppDbContext appDbContext = 
                        context.RequestServices.GetRequiredService<AppDbContext>();
                    await appDbContext.Database.EnsureDeletedAsync();

                    await context.Response.WriteAsync("Done!");
                    await context.Response.WriteAsync(Environment.NewLine);
                    await context.Response.WriteAsync("Navigate to /health to see the health status.");
                    await context.Response.WriteAsync(Environment.NewLine);
                });

                endpoints.MapGet("/{**path}", async context =>
                {
                    await context.Response.WriteAsync("Navigate to /health to see the health status.");
                    await context.Response.WriteAsync(Environment.NewLine);
                    await context.Response.WriteAsync("Navigate to /createdatabase to create the database.");
                    await context.Response.WriteAsync(Environment.NewLine);
                    await context.Response.WriteAsync("Navigate to /deletedatabase to delete the database.");
                    await context.Response.WriteAsync(Environment.NewLine);
                });
            });
        }
    }
}
