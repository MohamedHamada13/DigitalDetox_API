using DigitalDetox.Core.DTOs.Auth;
using DigitalDetox.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Web.Providers.Entities;

namespace DigitalDetox.API.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RsgisterAsync([FromBody]RegisterModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.RegisterAsync(model);
            if(!result.IsAuthenticated) 
                return BadRequest(result.FaildMessage);

            return Ok(new { result.UserName, result.Token, result.ExpiresOn, result.RefreshToken, result.RefreshTokenExpiration });
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.LoginAsync(model);
            
            if (!result.IsAuthenticated)
                return BadRequest(result.FaildMessage);

            return Ok(new { result.UserName, result.Token, result.ExpiresOn, result.RefreshToken, result.RefreshTokenExpiration });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshTokenAsync([FromBody] string token)
        {
            var result = await _authService.RefreshTokenAsync(token);

            if (!result.IsAuthenticated)
                return BadRequest(result.FaildMessage);

            return Ok(new { result.UserName, result.Token, result.ExpiresOn, result.RefreshToken, result.RefreshTokenExpiration });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> LogoutAsync([FromBody] LogoutRequestModel model)
        {
            var result = await _authService.LogoutAsync(model.refToken);

            if (!result)
                return Unauthorized();

            return Ok("Logged out successfully.");
        }


        [HttpPost("addrole")]
        public async Task<IActionResult> AddRoleAsync([FromBody] AddRoleModel model)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.AddRoleAsync(model);

            if (!string.IsNullOrEmpty(result))
                return BadRequest(result);

            return Ok(new { Message = "Role is added",  model});
        }
    }
}
