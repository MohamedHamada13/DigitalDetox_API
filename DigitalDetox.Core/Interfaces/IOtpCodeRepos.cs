using DigitalDetox.Core.Entities.AuthModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDetox.Core.Interfaces
{
    public interface IOtpCodeRepos : IGenericRepos<OtpCode>
    {
        Task<OtpCode?> GetOtpByEmail(string email);
    }
}
