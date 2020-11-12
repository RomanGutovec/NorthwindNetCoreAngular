using System;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Identity;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Northwind.Persistence;

namespace WebUI
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope()) {
                var services = scope.ServiceProvider;

                try {
                    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                    var northwindContext = services.GetRequiredService<NorthwindDbContext>();

                    if (northwindContext.Database.IsSqlServer() && !await northwindContext.Database.CanConnectAsync()) {
                        await northwindContext.Database.MigrateAsync();
                        var seeder = new NorthwindDbContextSeeder(northwindContext, userManager);
                        await seeder.SeedAllAsync(CancellationToken.None);
                    }

                } catch (Exception ex) {
                    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while migrating or initializing the database.");
                    throw;
                }
            }

            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
