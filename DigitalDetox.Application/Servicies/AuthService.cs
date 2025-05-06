using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DigitalDetox.Core.Entities.Models;
using DigitalDetox.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using DigitalDetox.Infrastructure.ExServices;
using DigitalDetox.Core.Entities.AuthModels;
using System.Web.Http;
using System.Data.Entity;
using DigitalDetox.Core.DTOs.OtpCodeDtos;
namespace DigitalDetox.Application.Servicies
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JWT _Jwt;
        private readonly IUserStoreTemporaryRepos _UserStoreCtx;
        private readonly IOtpCodeRepos _otpCodeRepos;

        public AuthService(UserManager<AppUser> userManager, IOptions<JWT> Jwt, RoleManager<IdentityRole> roleManager,
            IUserStoreTemporaryRepos ctx, IOtpCodeRepos otpCodeRepos)
        {
            _userManager = userManager;
            _Jwt = Jwt.Value;
            _roleManager = roleManager;
            _UserStoreCtx = ctx;
            _otpCodeRepos = otpCodeRepos;
        }

        // Send the verifing message in Gmail using smtp.
        #region Signup
        public async Task<SignUpResponse> InitSignUpAsync(SignUpReqModel model)
        {
            if (await _userManager.FindByEmailAsync(model.Email) != null)
                return new SignUpResponse { FaildMessage = "Email already registered" };

            if (await _userManager.FindByNameAsync(model.UserName) is not null)
                return new SignUpResponse { FaildMessage = $"username `{model.UserName}` is already used" };

            // 🔐 Validate the password
            foreach (var validator in _userManager.PasswordValidators)
            {
                var validationResult = await validator.ValidateAsync(_userManager, null!, model.Password);
                if (!validationResult.Succeeded)
                {
                    return new SignUpResponse
                    {
                        FaildMessage = string.Join(", ", validationResult.Errors.Select(e => e.Description))
                    };
                }
            }

            // check if the user is already in pending verification
            var existingPending = await _UserStoreCtx.GetByEmail(model.Email);

            var pendingUser = new UserStoreTemporary(model);
            if (existingPending == null)
            {
                await _UserStoreCtx.AddAsync(pendingUser);
            }
            else
            {
                // this update Code & expiration
                existingPending.Code = pendingUser.Code;
                existingPending.ExpiresAt = pendingUser.ExpiresAt;
                existingPending.UserName = pendingUser.UserName;
                existingPending.FirstName = pendingUser.FirstName;
                existingPending.LastName = pendingUser.LastName;
                existingPending.DateOfBirth = pendingUser.DateOfBirth;
                existingPending.Email = pendingUser.Email;
                existingPending.Password = pendingUser.Password;


                await _UserStoreCtx.UpdateAsync(existingPending);
            }

            // Send the message in Gmail using smtp
            EmailService.SendEmail(model.Email, "Your verification code", $"Your code is: {pendingUser.Code}");
            return new SignUpResponse { Email = model.Email, IsSuccess = true }; // return email to the enduser to send it agian in VerifyCodeAsync() endpoint.
        }

        public async Task<AuthModel> VerifyCodeAsync(string email, string inputCode)
        {
            var pendingUser = await _UserStoreCtx.GetByEmail(email);

            if (pendingUser == null) // mistake from enduser
                return new AuthModel { FaildMessage = "Invalid Email. [mistake from frontend]" };

            if (pendingUser.ExpiresAt < DateTime.UtcNow)
                return new AuthModel { FaildMessage = "Expired code." };

            if (pendingUser.Code != inputCode)
                return new AuthModel { FaildMessage = "Invalid code." };

            var newUser = new AppUser(pendingUser);

            var result = await _userManager.CreateAsync(newUser, pendingUser.Password);

            if (!result.Succeeded)
                return new AuthModel { FaildMessage = string.Join(", ", result.Errors.Select(e => e.Description)) };

            await _userManager.AddToRoleAsync(newUser, "User");

            var jwt = await GenerateJwtToken(newUser);
            var roles = await _userManager.GetRolesAsync(newUser);

            newUser.RefreshToken = GenerateRefreshToken();
            newUser.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _userManager.UpdateAsync(newUser);

            await _UserStoreCtx.RemoveAsync(pendingUser); // cleanup the user store.

            return new AuthModel
            {
                IsAuthenticated = true,
                UserName = newUser.UserName,
                Email = email,
                Token = new JwtSecurityTokenHandler().WriteToken(jwt),
                ExpiresOn = jwt.ValidTo.ToLocalTime(),
                Roles = roles.ToList(),
                RefreshToken = newUser.RefreshToken,
                RefreshTokenExpiration = newUser.RefreshTokenExpiryTime.ToLocalTime()
            };
        }
        #endregion

        public async Task<SignUpResponse> ReSendCode(string email)
        {
            var newCode = new Random().Next(100000, 999999).ToString();
            var user = await _UserStoreCtx.GetByEmail(email);

            if (user == null)
                return new SignUpResponse { FaildMessage = "Email is Incorrect. [mistake from frontend]" };

            user.Code = newCode;
            await _UserStoreCtx.UpdateAsync(user);

            try
            {
                EmailService.SendEmail(email, "Your verification code", $"Your code is: {newCode}");
            }
            catch
            {
                return new SignUpResponse { FaildMessage = "Email not send, try agian" };
            }

            return new SignUpResponse { Email = email, IsSuccess = true };
        }

        public async Task<AuthModel> LoginAsync(LoginModel model)
        {
            // Check if the user exists and get him
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user is null || !await _userManager.CheckPasswordAsync(user, model.Password))
                return new AuthModel { FaildMessage = "Email or password is incorrect" };

            // Create new token & Get roles
            var JwtToken = await GenerateJwtToken(user);
            var roles = await _userManager.GetRolesAsync(user);

            // Generate refresh token, then assign it to the user data
            user.RefreshToken = GenerateRefreshToken();
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _userManager.UpdateAsync(user);

            return new AuthModel
            {
                UserName = user.UserName,
                IsAuthenticated = true,
                Token = new JwtSecurityTokenHandler().WriteToken(JwtToken),
                ExpiresOn = JwtToken.ValidTo.ToLocalTime(),
                RefreshToken = user.RefreshToken,
                RefreshTokenExpiration = user.RefreshTokenExpiryTime.ToLocalTime(),
                Roles = roles.ToList()
            };
        }


        #region Reset Password 
        public async Task<SendOtpCodeResponse> SendOtpCode(SendOtpCodeRequest model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return new SendOtpCodeResponse { Message = "If the email exists, a code has been sent." }; // here

            var code = RandomNumberGenerator.GetInt32(100000, 999999).ToString();

            // Checkout if the otpCode is actually exist, and remove it before create a new one.
            var existingOtp = await _otpCodeRepos.GetOtpByEmail(model.Email);
            if (existingOtp != null)
                await _otpCodeRepos.DeleteAsync(existingOtp);

            // Create a new OtpCode
            var otp = new OtpCode
            {
                Email = model.Email,
                Code = code,
                ExpirationTime = DateTime.UtcNow.AddMinutes(10)
            };
            await _otpCodeRepos.AddAsync(otp);

            EmailService.SendEmail(model.Email, "Password Reset Code", $"Your reset code is: {code}");

            return new SendOtpCodeResponse { EmailIsExist = true, Message = "Code is sent to the email successfully" };
        }

        public async Task<OtpCodeResponse> OtpCodeChechout(OtpCodeRequest model)
        {
            var otp = await _otpCodeRepos.GetOtpByEmail(model.Email);
            if (otp == null)
                return new OtpCodeResponse { Message = "Invalid email" };

            if (otp.Code != model.Code)
                return new OtpCodeResponse { Message = "Code is incorrect." };

            if (otp.ExpirationTime < DateTime.UtcNow)
            {
                await _otpCodeRepos.DeleteAsync(otp);
                return new OtpCodeResponse { Message = "expired code, use SendOtpCode again" };
            }

            otp.IsChecked = true;
            await _otpCodeRepos.UpdateAsync(otp);
            return new OtpCodeResponse { IsTrue = true, Message = "Correct code." };
        }

        public async Task<ResetPasswordResponse> ResetPassword(ResetPasswordRequest model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return new ResetPasswordResponse { Message = "Invalid email, the email not registerd" };

            var otp = await _otpCodeRepos.GetOtpByEmail(model.Email);
            if (otp == null)
                return new ResetPasswordResponse { Message = "Verify your email first then Checkout the code, mother father" };

            if (!otp.IsChecked)
                return new ResetPasswordResponse { Message = "Checkout the code first mother father" };

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);

            if (!result.Succeeded)
                return new ResetPasswordResponse { Message = "Password reset failed." };

            await _otpCodeRepos.DeleteAsync(otp);

            return new ResetPasswordResponse { IsSuccess = true, Message = "Password has been updated" };
        }
        #endregion


        public async Task<AuthModel> RefreshTokenAsync(string token)
        {
            var user = await _userManager.Users
                .FirstOrDefaultAsync(u => u.RefreshToken == token);

            if (user == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return new AuthModel
                {
                    FaildMessage = "Invalid or expired refresh token"
                };
            }

            // Get user roles
            var roles = (await _userManager.GetRolesAsync(user)).ToList();

            // Create new JWTAccessToken + Refresh Token
            var jwtAccessToken = await GenerateJwtToken(user);
            var newRefreshToken = GenerateRefreshToken();
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _userManager.UpdateAsync(user);

            return new AuthModel
            {
                IsAuthenticated = true,
                UserName = user.UserName,
                Token = new JwtSecurityTokenHandler().WriteToken(jwtAccessToken),
                ExpiresOn = jwtAccessToken.ValidTo.ToLocalTime(),
                RefreshToken = newRefreshToken,
                RefreshTokenExpiration = user.RefreshTokenExpiryTime,
                Roles = roles
            };
        }

        public async Task<AuthModel> AddRoleAsync(AddRoleModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
                return new AuthModel { FaildMessage = "Invalid UserId" };

            if (!await _roleManager.RoleExistsAsync(model.RoleName))
                return new AuthModel { FaildMessage = "Role is invalid" };

            if (await _userManager.IsInRoleAsync(user, model.RoleName))
                return new AuthModel { FaildMessage = "User already assigned to this role" };

            var result = await _userManager.AddToRoleAsync(user, model.RoleName);
            if (!result.Succeeded)
                return new AuthModel { FaildMessage = "Something went wrong while assigning role" };

            //  Re-generate token now that roles have changed
            var jwtToken = await GenerateJwtToken(user);
            var roles = await _userManager.GetRolesAsync(user);

            return new AuthModel
            {
                IsAuthenticated = true,
                UserName = user.UserName,
                Roles = roles.ToList(),
                Token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                ExpiresOn = jwtToken.ValidTo.ToLocalTime(),
                RefreshToken = user.RefreshToken,
                RefreshTokenExpiration = user.RefreshTokenExpiryTime.ToLocalTime()
            };
        }

        private async Task<JwtSecurityToken> GenerateJwtToken(AppUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim(ClaimTypes.Role, role));

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symetricSecKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_Jwt.Key));
            var siginingCredentials = new SigningCredentials(symetricSecKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
            issuer: _Jwt.Issuer,
            audience: _Jwt.Audience,
            expires: DateTime.UtcNow.AddMinutes(_Jwt.DurationInMinutes),
            claims: claims,
            signingCredentials: siginingCredentials);

            return token;
        }

        public async Task<bool> LogoutAsync(string userRefreshToken)
        {
            var user = await _userManager.Users
                .FirstOrDefaultAsync(u => u.RefreshToken == userRefreshToken);

            if (user == null) 
                return false;

            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            return true;
        }

        private string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }
    }
}
