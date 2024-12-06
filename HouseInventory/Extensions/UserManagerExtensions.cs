using HouseInventory.Data.Context;
using HouseInventory.Data.Entities;
using HouseInventory.Models;
using HouseInventory.Models.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Extensions;

namespace HouseInventory.Extensions
{
    public static class UserManagerExtensions
    {
        public static async Task<User?> CreateUserFromSocialLogin(this UserManager<User> userManager, ApplicationDbContext context, UserFromSocialLogin socialUser, LoginProvider loginProvider)
        {
            var providerName = loginProvider.GetDisplayName();

            // Check if user is already linked to the login provider
            var userWithLogin = await userManager.FindByLoginAsync(providerName, socialUser.LoginProviderSubject);

            if (userWithLogin is not null)
            {
                return userWithLogin;
            }

            // Check if the user is already registered
            var userWithEmail = await userManager.FindByEmailAsync(socialUser.Email);

            if (userWithEmail is not null)
            {
                throw new InvalidOperationException("This email is already registered in the system. Please sign in using your existing account.");
            }

            var newUser = new User
            {
                FirstName = socialUser.FirstName,
                LastName = socialUser.LastName,
                Email = socialUser.Email,
                UserName = socialUser.Email,
                EmailConfirmed = true
            };

            var hasCreatedUser = await userManager.CreateAsync(newUser);
            if (!hasCreatedUser.Succeeded)
            {
                throw new InvalidOperationException("Failed to create a new user.");
            }

            // Add login info
            var newUserLoginInfo = new UserLoginInfo(providerName, socialUser.LoginProviderSubject, providerName.ToUpper());
            var addLoginResult = await userManager.AddLoginAsync(newUser, newUserLoginInfo);

            if (!addLoginResult.Succeeded)
            {
                throw new InvalidOperationException("Failed to associate the login with the new user.");
            }

            await context.SaveChangesAsync();

            return newUser;
        }
    }
}
