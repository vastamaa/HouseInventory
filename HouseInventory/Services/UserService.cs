using AutoMapper;
using HouseInventory.Data.Entities;
using HouseInventory.Models.DTOs;
using HouseInventory.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics.CodeAnalysis;

namespace HouseInventory.Services
{
    public sealed class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public UserService(UserManager<User> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<IdentityResult> RegisterUserAsync(UserRegistrationDto userRegistrationDto)
        {
            var user = _mapper.Map<User>(userRegistrationDto);

            var result = await _userManager.CreateAsync(user, userRegistrationDto.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRolesAsync(user, userRegistrationDto.Roles);
            }

            return result;
        }

        public async Task<bool> ValidateUserCredentialsAsync(UserLoginDto userLoginDto)
        {
            var user = await FindUserByEmailAsync(userLoginDto.Email);
            return user is not null && await CheckPasswordAsync(user, userLoginDto.Password);
        }

        [ExcludeFromCodeCoverage]
        public async Task<User> FindUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        [ExcludeFromCodeCoverage]
        public async Task<User> FindUserByNameAsync(string name)
        {
            return await _userManager.FindByNameAsync(name);
        }

        [ExcludeFromCodeCoverage]
        public async Task<IList<string>> GetUserRolesAsync(User user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        [ExcludeFromCodeCoverage]
        public async Task UpdateUserAsync(User user)
        {
            await _userManager.UpdateAsync(user);
        }

        [ExcludeFromCodeCoverage]
        private async Task<bool> CheckPasswordAsync(User user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }
    }
}
