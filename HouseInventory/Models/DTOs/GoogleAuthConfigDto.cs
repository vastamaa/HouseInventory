using System.Diagnostics.CodeAnalysis;

namespace HouseInventory.Models.DTOs
{
    [ExcludeFromCodeCoverage]
    public record GoogleAuthConfigDto
    {
        public string ClientId { get; init; }
        public string ClientSecret { get; init; }
    }
}
