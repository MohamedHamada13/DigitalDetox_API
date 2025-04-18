using Azure.Core;
using DigitalDetox.Core.Entities.AuthModels;
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
        Task<AuthModel> LoginAsync(LoginModel model);
        Task<AuthModel> RefreshTokenAsync(string token);
        Task<bool> LogoutAsync(string refreshToken);
        Task<AuthModel> AddRoleAsync(AddRoleModel model);
        Task<SignUpResponse> InitSignUpAsync(SignUpReqModel model);
        Task<AuthModel> VerifyCodeAsync(string email, string inputCode);
        Task<SignUpResponse> ReSendCode(string email);
    }
}
