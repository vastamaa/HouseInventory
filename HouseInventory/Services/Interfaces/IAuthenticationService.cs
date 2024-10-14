using HouseInventory.Models.DTOs;
using Microsoft.AspNetCore.Identity;

namespace HouseInventory.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<IdentityResult> RegisterUserAsync(UserRegistrationDto userRegistration);
        Task<bool> ValidateUserAsync(UserLoginDto user);
        Task<TokenDto> CreateTokenAsync(bool populateExpiration);
        Task<TokenDto> RefreshTokenAsync(TokenDto tokenDto);
        Task LogoutUserAsync();
    }
}
