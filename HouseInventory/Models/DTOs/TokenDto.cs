using System.Diagnostics.CodeAnalysis;

namespace HouseInventory.Models.DTOs
{
    [ExcludeFromCodeCoverage]
    public record TokenDto(string AccessToken, string RefreshToken);
}
