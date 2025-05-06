using DigitalDetox.Core.DTOs;
using DigitalDetox.Core.DTOs.OtpCodeDtos;
using DigitalDetox.Core.Entities.AuthModels;
using DigitalDetox.Core.Interfaces;
using DigitalDetox.Infrastructure.ExServices;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Web.Providers.Entities;

namespace DigitalDetox.API.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("SignUp")]
        public async Task<IActionResult> InitialSignUpAsync([FromBody] SignUpReqModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.InitSignUpAsync(model);
            if (!result.IsSuccess)
                return BadRequest(result.FaildMessage);

            return Ok(result);
        }

        [HttpPost("VerifyCode")]
        public async Task<IActionResult> VerifyCodeAsync([FromBody] VerifyCodeModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(await _authService.VerifyCodeAsync(model.Email, model.Code));
        }

        [HttpPost("ResendCode")]
        public async Task<IActionResult> ResendCodeAsync([FromBody] ResendCodeReqModel model)
        {
            var result = await _authService.ReSendCode(model.Email);

            if(!result.IsSuccess) 
                return BadRequest(result.FaildMessage);

            return Ok(result);
        }

        
        [HttpPost("SendOtpCode")]
        [EnableRateLimiting("OtpPolicy")]
        public async Task<IActionResult> SendOtpCode([FromBody] SendOtpCodeRequest model)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.SendOtpCode(model);
            if(!result.EmailIsExist)
                return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpPost("OtpCodeChechout")]
        [EnableRateLimiting("OtpPolicy")]
        public async Task<IActionResult> OtpCodeChechout([FromBody] OtpCodeRequest model)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.OtpCodeChechout(model);
            if(!result.IsTrue)
                return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.ResetPassword(model);
            if(!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.LoginAsync(model);
            
            if (!result.IsAuthenticated)
                return Unauthorized(result.FaildMessage);

            //return Ok(new { result.UserName, result.Token, result.ExpiresOn, result.RefreshToken, result.RefreshTokenExpiration });
            return Ok(result);
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

            if (!result.IsAuthenticated)
                return BadRequest(result.FaildMessage);

            return Ok(result);
        }
    }
}
