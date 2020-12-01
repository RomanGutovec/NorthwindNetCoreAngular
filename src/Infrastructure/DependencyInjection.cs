using Application.Common.Interfaces;
using Infrastructure.Common.Processors;
using Infrastructure.Identity;
using Infrastructure.Mailing;
using Infrastructure.Mailing.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddNorthwindPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<NorthwindDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("NorthwindDatabase")));

            services.AddScoped<INorthwindDbContext>(provider => provider.GetService<NorthwindDbContext>());

            services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<NorthwindDbContext>();

            services.AddIdentityServer()
                .AddApiAuthorization<ApplicationUser, NorthwindDbContext>();

            services.AddTransient<ICategoryImageProcessor, CategoryImageProcessor>();

            //services.AddAuthentication()
            //    .AddIdentityServerJwt();

            services.Configure<SmtpSettings>(configuration.GetSection("SmtpSettings"));
            services.AddSingleton<IEmailService, EmailService>();

            return services;
        }
    }
}
