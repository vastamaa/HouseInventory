using HouseInventory.Models.DTOs;
using Microsoft.AspNetCore.Identity;

namespace HouseInventory.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<IdentityResult> RegisterUserAsync(UserRegistrationDto userRegistration);
        Task<bool> ValidateUserAsync(UserLoginDto userForAuthentication);
        Task<string> CreateTokenAsync();
        Task LogoutUserAsync();
    }
}
