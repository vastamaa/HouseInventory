using HouseInventory.Data.Context;
using HouseInventory.Data.Entities;
using Microsoft.AspNetCore.Identity;

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
    }
}
