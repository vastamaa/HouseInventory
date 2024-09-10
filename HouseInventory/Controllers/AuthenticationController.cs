using HouseInventory.Models.DTOs;
using HouseInventory.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HouseInventory.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ILoggerManager _logger;

        public AuthenticationController(IAuthenticationService authenticationService, ILoggerManager logger)
        {
            _authenticationService = authenticationService;
            _logger = logger;
        }

        [HttpPost(nameof(RegisterUser))]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegistrationDto userForRegistration)
        {
            var result = await _authenticationService.RegisterUserAsync(userForRegistration);

            if (!result.Succeeded)
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }
                return BadRequest(ModelState);
            }

            _logger.LogInfo("User successfully created!");

            return Created();
        }

        [HttpPost(nameof(LoginUser))]
        public async Task<IActionResult> LoginUser([FromBody] UserLoginDto userForLogin)
        {
            var result = await _authenticationService.LoginUserAsync(userForLogin);

            return result.Succeeded ? Ok("Logged in!") : BadRequest(ModelState);
        }

        [HttpPost(nameof(LogoutUser))]
        public async Task<IActionResult> LogoutUser()
        {
            await _authenticationService.LogoutUserAsync();

            _logger.LogInfo("User logged out successfully!");

            return Ok("Logged out!");
        }
    }
}
