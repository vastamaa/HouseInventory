using HouseInventory.Data.Context;
using HouseInventory.Data.Entities;
using HouseInventory.Services;
using HouseInventory.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace HouseInventory.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCustomIdentityConfiguration(this IServiceCollection services)
        {
            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 10;
                options.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
        }

        public static void AddDatabaseConnection(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });
        }

        public static void AddCustomServices(this IServiceCollection services)
        {
            services.AddScoped<Services.Interfaces.IAuthenticationService, Services.AuthenticationService>();
        }
    }
}
