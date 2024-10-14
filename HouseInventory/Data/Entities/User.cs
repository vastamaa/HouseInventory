using Microsoft.AspNetCore.Identity;
using System.Diagnostics.CodeAnalysis;

namespace HouseInventory.Data.Entities
{
    [ExcludeFromCodeCoverage]
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
