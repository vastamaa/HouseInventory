using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace HouseInventory.Models.DTOs
{
    [ExcludeFromCodeCoverage]
    public class UserRegistrationDto
    {
        public string? FirstName { get; init; }
        public string? LastName { get; init; }
        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; init; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; init; }
        public string Email { get; init; }
        public string? PhoneNumber { get; init; }
        public ICollection<string> Roles { get; } = new Collection<string> { nameof(Models.Roles.Member) };
    }

    [ExcludeFromCodeCoverage]
    public class UserLoginDto
    {
        [Required(ErrorMessage = "Email is requried!")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is requried!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}