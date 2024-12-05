using HouseInventory.Models.DTOs;

namespace HouseInventory.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<bool> ValidateUserAsync(UserLoginDto user);
        Task<TokenDto> CreateTokenAsync(bool populateExpiration);
        Task<TokenDto> RefreshTokenAsync(TokenDto tokenDto);
        Task LogoutUserAsync();
    }
}
