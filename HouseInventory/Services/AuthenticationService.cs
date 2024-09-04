using AutoMapper;
using HouseInventory.Data.Entities;
using HouseInventory.Models.DTOs;
using HouseInventory.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace HouseInventory.Services
{
    public sealed class AuthenticationService : IAuthenticationService
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public AuthenticationService(IMapper mapper, UserManager<User> userManager)
        {
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<IdentityResult> RegisterUser(UserForRegistrationDto userRegistration)
        {
            var user = _mapper.Map<User>(userRegistration);

            var result = await _userManager.CreateAsync(user, userRegistration.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRolesAsync(user, userRegistration.Roles);
            }

            return result;
        }
    }
}
