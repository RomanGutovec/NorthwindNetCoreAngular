using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Application;
using Application.Products.Commands.UpdateProduct;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using WebUI.MVC.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Infrastructure.Persistence;
using Newtonsoft.Json.Serialization;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.Logging;

namespace WebUI.MVC
{
    public class Startup
    {
        private readonly ILogger<Startup> _logger;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            //_logger = logger;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddNorthwindPersistence(Configuration);
            services.AddNorthwindApplication();

            //logger.LogInformation(string.Join(Environment.NewLine, Configuration.AsEnumerable().Select((k, v) => $"{k.Key} - {k.Value}")));

            services.AddMvc(setup =>
            {

            }).AddNewtonsoftJson(options =>
                options.SerializerSettings.ContractResolver =
                    new CamelCasePropertyNamesContractResolver()).AddFluentValidation();

            services.AddControllersWithViews()
                .AddRazorRuntimeCompilation();
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            logger.LogInformation(string.Join(Environment.NewLine, Configuration.AsEnumerable().Select((k, v) => $"{k.Key} - {k.Value}")));
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            } else if(env.IsStaging()){
                app.UseExceptionHandler("/Error");
            } else {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}