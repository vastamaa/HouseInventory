using HouseInventory.Data.Entities;
using System.Security.Claims;

namespace HouseInventory.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateAccessToken(User user, IList<string> roles);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        string GenerateRefreshToken();
    }
}
