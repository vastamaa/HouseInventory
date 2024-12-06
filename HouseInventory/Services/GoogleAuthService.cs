using HouseInventory.Data.Context;
using HouseInventory.Data.Entities;
using HouseInventory.Extensions;
using HouseInventory.Models;
using HouseInventory.Models.DTOs;
using HouseInventory.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using static Google.Apis.Auth.GoogleJsonWebSignature;

namespace HouseInventory.Services
{
    public sealed class GoogleAuthService : IGoogleAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly GoogleAuthConfigDto _googleAuthConfig;
        private readonly ILoggerManager _logger;
        private readonly UserManager<User> _userManager;

        public GoogleAuthService(ApplicationDbContext context, IOptions<GoogleAuthConfigDto> googleAuthConfig, ILoggerManager logger, UserManager<User> userManager)
        {
            _context = context;
            _googleAuthConfig = googleAuthConfig.Value;
            _logger = logger;
            _userManager = userManager;
        }

        public async Task<User> GoogleSignInAsync(GoogleSignInVMDto googleSignInDto)
        {
            var payload = new Payload();

            try
            {
               payload = await ValidateAsync(googleSignInDto.IdToken, new ValidationSettings
               {
                  Audience = new[] { _googleAuthConfig.ClientId }
               });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new InvalidOperationException("Failed to validate the Google token.", ex);
            }

            var userToBeCreated = new UserFromSocialLogin
            {
                FirstName = payload.GivenName,
                LastName = payload.FamilyName,
                Email = payload.Email,
                LoginProviderSubject = payload.Subject,
            };

            var user = await _userManager.CreateUserFromSocialLogin(_context, userToBeCreated, LoginProvider.Google);

            return user;
        }
    }
}
