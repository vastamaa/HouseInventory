using System.ComponentModel.DataAnnotations;

namespace HouseInventory.Models.DTOs
{
    public record GoogleSignInVMDto
    {
        [Required]
        public string IdToken { get; init; }
    }
}
