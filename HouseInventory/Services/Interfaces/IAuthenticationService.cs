using HouseInventory.Models.DTOs;
using Microsoft.AspNetCore.Identity;

namespace HouseInventory.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<IdentityResult> RegisterUser(UserForRegistrationDto userRegistration);
    }
}
