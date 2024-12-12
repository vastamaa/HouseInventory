using HouseInventory.Data.Entities;
using HouseInventory.Data.Entities.Exceptions;
using HouseInventory.Models.DTOs;
using HouseInventory.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.CodeAnalysis;

namespace HouseInventory.Services
{
    public sealed class AuthenticationService : IAuthenticationService
    {
        private readonly ITokenService _tokenService;
        private readonly IUserService _userService;
        private readonly SignInManager<User> _signInManager;
        private readonly ILoggerManager _logger;

        public AuthenticationService(SignInManager<User> signInManager, ILoggerManager logger, ITokenService tokenService, IUserService userService)
        {
            _signInManager = signInManager;
            _logger = logger;
            _tokenService = tokenService;
            _userService = userService;
        }

        [ExcludeFromCodeCoverage]
        public async Task LogoutUserAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<User> ValidateUserAsync(UserLoginDto loginUser)
        {
            var isValidUser = await _userService.ValidateUserCredentialsAsync(loginUser);

            if (!isValidUser)
            {
                _logger.LogWarn($"{nameof(ValidateUserAsync)}: Authentication failed. Wrong user name or password.");
            }

            var user = await _userService.FindUserByEmailAsync(loginUser.Email);

            if (user is null)
            {
                _logger.LogWarn($"{nameof(ValidateUserAsync)}: User not found after validation.");
            }

            return user;
        }

        public async Task<TokenDto> CreateTokenAsync(User user, bool populateExpiration)
        {
            var roles = await _userService.GetUserRolesAsync(user);
            var accessToken = _tokenService.GenerateAccessToken(user, roles);
            var refreshToken = _tokenService.GenerateRefreshToken();

            if (populateExpiration)
            {
                user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
            }

            user.RefreshToken = refreshToken;
            await _userService.UpdateUserAsync(user);

            return new TokenDto(accessToken, refreshToken);
        }

        public async Task<TokenDto> RefreshTokenAsync(TokenDto tokenDto)
        {
            // Validate and extract claims from expired access token
            var principal = _tokenService.GetPrincipalFromExpiredToken(tokenDto.AccessToken) ?? throw new SecurityTokenException("Invalid access token.");
            var user = await _userService.FindUserByNameAsync(principal.Identity.Name);

            if (user is null || user.RefreshToken != tokenDto.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                throw new RefreshTokenBadRequestException();
            }

            return await CreateTokenAsync(user, populateExpiration: false);
        }
    }
}
