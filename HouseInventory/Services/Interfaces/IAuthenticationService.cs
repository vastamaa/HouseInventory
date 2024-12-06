using HouseInventory.Data.Entities;
using HouseInventory.Models.DTOs;

namespace HouseInventory.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<User> ValidateUserAsync(UserLoginDto loginUser);
        Task<TokenDto> CreateTokenAsync(User user, bool populateExpiration);
        Task<TokenDto> RefreshTokenAsync(TokenDto tokenDto);
        Task LogoutUserAsync();
    }
}
