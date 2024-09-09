﻿using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace HouseInventory.Models.DTOs
{
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
        public ICollection<string> Roles { get; } = new Collection<string> { nameof(RolesEnum.Member) };
    }

    public class UserLoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}