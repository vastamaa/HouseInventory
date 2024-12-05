﻿using HouseInventory.ActionFilters;
using HouseInventory.Models.DTOs;
using HouseInventory.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace HouseInventory.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ExcludeFromCodeCoverage]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserService _userService;
        private readonly ILoggerManager _logger;

        public AuthenticationController(IAuthenticationService authenticationService, ILoggerManager logger, IUserService userService)
        {
            _authenticationService = authenticationService;
            _logger = logger;
            _userService = userService;
        }

        [HttpPost(nameof(RegisterUser))]
        [ServiceFilter(typeof(ValidationFilterAttribute))]  
        public async Task<IActionResult> RegisterUser([FromBody] UserRegistrationDto userForRegistration)
        {
            var result = await _userService.RegisterUserAsync(userForRegistration);

            if (!result.Succeeded)
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }
                return BadRequest(ModelState);
            }

            _logger.LogInfo("User successfully created!");
            return CreatedAtAction(nameof(RegisterUser), new { message = "User registrated!" });
        }

        [HttpPost(nameof(LoginUser))]
        public async Task<IActionResult> LoginUser([FromBody] UserLoginDto userForLogin)
        {
            if (!await _authenticationService.ValidateUserAsync(userForLogin))
            {
                return Unauthorized();
            }

            var tokenDto = await _authenticationService.CreateTokenAsync(populateExpiration: true);
            return Ok(tokenDto);
        }

        [HttpPost(nameof(LogoutUser))]
        public async Task<IActionResult> LogoutUser()
        {
            await _authenticationService.LogoutUserAsync();
            _logger.LogInfo("User logged out successfully!");

            return Ok(new { message = "User signed out!" });
        }
    }
}
