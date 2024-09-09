using AutoMapper;
using HouseInventory.Data.Entities;
using HouseInventory.Models.DTOs;
using Microsoft.AspNetCore.Identity;

namespace HouseInventory.Services
{
    public sealed class AuthenticationService : Interfaces.IAuthenticationService
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AuthenticationService(IMapper mapper, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IdentityResult> RegisterUserAsync(UserRegistrationDto userRegistration)
        {
            var user = _mapper.Map<User>(userRegistration);

            var result = await _userManager.CreateAsync(user, userRegistration.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRolesAsync(user, userRegistration.Roles);
            }

            return result;
        }

        public async Task<SignInResult> LoginUserAsync(UserLoginDto userLogin)
        {
            var user = await _userManager.FindByEmailAsync(userLogin.Email);

            var result = await _signInManager
                .PasswordSignInAsync(user.UserName, userLogin.Password, userLogin.RememberMe, false);

            return result;
        }

        public async Task LogoutUserAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
