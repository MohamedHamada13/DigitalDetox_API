using DigitalDetox.Core.Context;
using DigitalDetox.Core.Entities.AuthModels;
using DigitalDetox.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDetox.Infrastructure.Persistance.Repositories
{
    public class OtpCodeRepos : GenericRepos<OtpCode>, IOtpCodeRepos
    {
        private readonly DegitalDbContext _ctx;
        public OtpCodeRepos(DegitalDbContext ctx) : base(ctx)
        {
            _ctx = ctx;
        }

        public async Task<OtpCode?> GetOtpByEmail(string email) => 
            await _ctx.OtpCodes
                .FirstOrDefaultAsync(otp => otp.Email == email);

    }
}
