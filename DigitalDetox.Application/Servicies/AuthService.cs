using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DigitalDetox.Core.DTOs.Auth;
using DigitalDetox.Core.Entities.Auth;
using DigitalDetox.Core.Entities.Models;
using DigitalDetox.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using System.Web.Providers.Entities;
using System.Runtime.InteropServices;

namespace DigitalDetox.Application.Servicies
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JWT _Jwt;
        public AuthService(UserManager<AppUser> userManager, IOptions<JWT> Jwt, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _Jwt = Jwt.Value;
            _roleManager = roleManager;
        }

        public async Task<AuthModel> RegisterAsync(RegisterModel model)
        {
            // Ensure that the user not in the DB 
            if (await _userManager.FindByEmailAsync(model.Email) is not null)
                return new AuthModel { FaildMessage = "Email is already registered" };

            if (await _userManager.FindByNameAsync(model.UserName) is not null)
                return new AuthModel { FaildMessage = $"{model.UserName} user name is already registered" };

            // Map the `RegisterModel` into `AppUser`.
            AppUser newUser = new AppUser(model);

            // Create a new User with hashd password
            var result = await _userManager.CreateAsync(newUser, model.Password);

            if (!result.Succeeded)
                return new AuthModel { FaildMessage = string.Join(", ", result.Errors.Select(e => e.Description)) };

            // Add the new user to a specific role (User)
            await _userManager.AddToRoleAsync(newUser, "User");

            // Generate JWT token for the created user & get his roles
            var JwtToken = await GenerateJwtToken(newUser);
            var roles = await _userManager.GetRolesAsync(newUser);

            // Generate refresh token, then assign it to the user data
            newUser.RefreshToken = GenerateRefreshToken();
            newUser.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _userManager.UpdateAsync(newUser);

            // Return the registered user data includes token & roles
            return new AuthModel
            {
                ExpiresOn = JwtToken.ValidTo.ToLocalTime(), // To imitates the pc local time
                IsAuthenticated = true,
                Roles = roles.ToList(),
                Token = new JwtSecurityTokenHandler().WriteToken(JwtToken),
                UserName = newUser.UserName,
                RefreshToken = newUser.RefreshToken,
                RefreshTokenExpiration = newUser.RefreshTokenExpiryTime.ToLocalTime()
            };
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

        public async Task<string> AddRoleAsync(AddRoleModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
                return "invalid UserId";

            if (!await _roleManager.RoleExistsAsync(model.RoleName))
                return "Role Is invalid";

            if (await _userManager.IsInRoleAsync(user, model.RoleName))
                return "User already assign to this role";

            var result = await _userManager.AddToRoleAsync(user, model.RoleName);

            return result.Succeeded ? string.Empty : "Something went wrong";
        }

        public async Task<bool> LogoutAsync(string refToken)
        {
            var user = await _userManager.Users
                .FirstOrDefaultAsync(u => u.RefreshToken == refToken);

            if (user == null) 
                return false;

            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            return true;
        }

        private async Task<JwtSecurityToken> GenerateJwtToken(AppUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

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

        private string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }

    }
}
