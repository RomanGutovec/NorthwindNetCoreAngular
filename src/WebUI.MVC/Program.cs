using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Identity;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using Northwind.Persistence;

namespace WebUI.MVC
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            var logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            logger.Info(Directory.GetCurrentDirectory());
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
                    logger.Error(ex, "An error occurred while migrating or initializing the database.");
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
                })
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(LogLevel.Information);
                }).UseNLog();
    }
}