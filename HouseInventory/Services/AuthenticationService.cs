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

        private User _user;

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

        public async Task<bool> ValidateUserAsync(UserLoginDto user)
        {
            var isUserValid = await _userService.ValidateUserCredentialsAsync(user);

            if (!isUserValid)
            {
                _logger.LogWarn($"{nameof(ValidateUserAsync)}: Authentication failed. Wrong user name or password.");
            }

            return isUserValid;
        }

        public async Task<TokenDto> CreateTokenAsync(bool populateExpiration)
        {
            var roles = await _userService.GetUserRolesAsync(_user);
            var accessToken = _tokenService.GenerateAccessToken(_user, roles);
            var refreshToken = _tokenService.GenerateRefreshToken();

            if (populateExpiration)
            {
                _user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
            }

            _user.RefreshToken = refreshToken;
            await _userService.UpdateUserAsync(_user);

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
            _user = user;
            
            return await CreateTokenAsync(populateExpiration: false);
        }
    }
}
