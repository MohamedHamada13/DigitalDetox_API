using DigitalDetox.Core.Context;
using DigitalDetox.Core.Entities.Models;
using DigitalDetox.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDetox.Infrastructure.Persistance.Repositories
{
    public class DailyUsageLogRepos : GenericRepos<DailyUsageLog>, IDailyUsageLogRepos
    {
        public DailyUsageLogRepos(DegitalDbContext ctx) : base(ctx)
        {
        }


    }
}
