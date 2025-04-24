using DigitalDetox.Core.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDetox.Core.Interfaces
{
    public interface IDailyUsageLogRepos : IGenericRepos<UserUsageLog>
    {
        public Task<UserUsageLog?> GetLogByUserAppDate(string userId, int appId, DateOnly logDate);
        public IQueryable<UserUsageLog>? GetDailyLogs(string userId, DateOnly dayDate);
        public IQueryable<UserUsageLog>? GetLogsInRange(string userId, DateOnly startOfRange, DateOnly endOfRange);
    }
}
