using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace HouseInventory.Models.DTOs
{
    [ExcludeFromCodeCoverage]
    public record GoogleSignInVMDto
    {
        [Required]
        public string IdToken { get; init; }
    }
}
