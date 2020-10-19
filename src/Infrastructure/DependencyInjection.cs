using Application.Common.Interfaces;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication;
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
                .AddEntityFrameworkStores<NorthwindDbContext>();

            services.AddIdentityServer()
                .AddApiAuthorization<ApplicationUser, NorthwindDbContext>();

            services.AddAuthentication()
                .AddIdentityServerJwt();

            return services;
        }
    }
}
