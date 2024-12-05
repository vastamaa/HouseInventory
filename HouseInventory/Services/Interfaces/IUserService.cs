using HouseInventory.Data.Entities;
using HouseInventory.Models.DTOs;
using Microsoft.AspNetCore.Identity;

namespace HouseInventory.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> FindUserByEmailAsync(string email);
        Task<User> FindUserByNameAsync(string name);
        Task<IList<string>> GetUserRolesAsync(User user);
        Task UpdateUserAsync(User user);
        Task<IdentityResult> RegisterUserAsync(UserRegistrationDto userRegistrationDto);
        Task<bool> ValidateUserCredentialsAsync(UserLoginDto userLoginDto);
    }
}
