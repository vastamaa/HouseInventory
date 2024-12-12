using HouseInventory.ActionFilters;
using HouseInventory.Models.DTOs;
using HouseInventory.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace HouseInventory.Controllers
{
    [Route("api/token")]
    [ApiController]
    [ExcludeFromCodeCoverage]
    public class TokenController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        public TokenController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("refresh")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Refresh([FromBody] TokenDto tokenDto)
        {
            var tokenDtoToReturn = await _authenticationService.RefreshTokenAsync(tokenDto);
            return Ok(tokenDtoToReturn);
        }
    }
}
