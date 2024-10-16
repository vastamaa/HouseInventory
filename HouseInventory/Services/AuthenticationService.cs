using AutoMapper;
using HouseInventory.Data.Entities;
using HouseInventory.Data.Entities.Exceptions;
using HouseInventory.Models.DTOs;
using HouseInventory.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace HouseInventory.Services
{
    public sealed class AuthenticationService : Interfaces.IAuthenticationService
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILoggerManager _logger;

        private User _user;

        public AuthenticationService(IMapper mapper, UserManager<User> userManager, SignInManager<User> signInManager, ILoggerManager logger, IConfiguration configuration)
        {
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _configuration = configuration;
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

        public async Task LogoutUserAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<bool> ValidateUserAsync(UserLoginDto user)
        {
            _user = await _userManager.FindByEmailAsync(user.Email);

            var result = _user != null && await _userManager.CheckPasswordAsync(_user, user.Password);

            if (!result)
            {
                _logger.LogWarn($"{nameof(ValidateUserAsync)}: Authentication failed. Wrong user name or password.");
            }

            return result;
        }

        [ExcludeFromCodeCoverage]
        public async Task<TokenDto> CreateTokenAsync(bool populateExpiration)
        {
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims();
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);

            var refreshToken = GenerateRefreshToken();

            _user.RefreshToken = refreshToken;

            if (populateExpiration)
            {
                _user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
            }

            await _userManager.UpdateAsync(_user);

            var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return new TokenDto(accessToken, refreshToken);
        }

        [ExcludeFromCodeCoverage]
        private SigningCredentials GetSigningCredentials()
        {
            var jwtSettings = _configuration.GetSection("Authentication");

            var key = Encoding.UTF8.GetBytes(jwtSettings["JWT_Secret"]);
            var secret = new SymmetricSecurityKey(key);

            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        [ExcludeFromCodeCoverage]
        private async Task<List<Claim>> GetClaims()
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, _user.UserName),
                new Claim(ClaimTypes.Email, _user.Email)
            };

            var roles = await _userManager.GetRolesAsync(_user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }

        [ExcludeFromCodeCoverage]
        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");

            return new JwtSecurityToken
            (
                issuer: jwtSettings["validIssuer"],
                audience: jwtSettings["validAudience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["expires"])),
                signingCredentials: signingCredentials
            );
        }

        [ExcludeFromCodeCoverage]
        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var randNumGen = RandomNumberGenerator.Create())
            {
                randNumGen.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        [ExcludeFromCodeCoverage]
        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var jwtSettings = _configuration.GetSection("Authentication");

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["JWT_Secret"])),
                ValidateLifetime = true,
                ValidIssuer = jwtSettings["validIssuer"],
                ValidAudience = jwtSettings["validAudience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);

            var jwtSecurityToken = securityToken as JwtSecurityToken;

            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }

        [ExcludeFromCodeCoverage]
        public async Task<TokenDto> RefreshTokenAsync(TokenDto tokenDto)
        {
            var principal = GetPrincipalFromExpiredToken(tokenDto.AccessToken);
            var user = await _userManager.FindByNameAsync(principal.Identity.Name);

            if (user == null || user.RefreshToken != tokenDto.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                throw new RefreshTokenBadRequestException();
            }
            _user = user;
            
            return await CreateTokenAsync(populateExpiration: false);
        }
    }
}
