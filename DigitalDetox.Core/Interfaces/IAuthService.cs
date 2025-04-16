using Azure.Core;
using DigitalDetox.Core.DTOs.Auth;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDetox.Core.Interfaces
{
    public interface IAuthService
    {
        Task<AuthModel> RegisterAsync(RegisterModel model);
        Task<AuthModel> LoginAsync(LoginModel model);
        Task<AuthModel> RefreshTokenAsync(string token);
        Task<bool> LogoutAsync(string refreshToken);
        Task<string> AddRoleAsync(AddRoleModel model);
    }
}
