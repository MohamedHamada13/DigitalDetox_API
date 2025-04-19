using DigitalDetox.Core.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDetox.Core.Interfaces
{
    public interface IDailyUsageLogService
    {
        Task LogUsageAsync(string userId, string app, TimeSpan usageTime, DateTime logDate);


        Task<IEnumerable<DailyUsageLog>> GetDailyLogsAsync(string userId, DateTime date);

        Task<IEnumerable<DailyUsageLog>> GetWeeklyLogsAsync(string userId, DateTime startOfWeek);

        Task<IEnumerable<DailyUsageLog>> GetMonthlyLogsAsync(string userId, int month, int year);

        Task<TimeSpan> GetTotalUsageAsync(string userId);


    }
}
