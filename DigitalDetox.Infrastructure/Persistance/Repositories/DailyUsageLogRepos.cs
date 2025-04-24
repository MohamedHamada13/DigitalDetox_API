using DigitalDetox.Core.Context;
using DigitalDetox.Core.Entities.Models;
using DigitalDetox.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDetox.Infrastructure.Persistance.Repositories
{
    public class DailyUsageLogRepos : GenericRepos<UserUsageLog>, IDailyUsageLogRepos
    {
        private readonly DegitalDbContext _ctx;
        public DailyUsageLogRepos(DegitalDbContext ctx) : base(ctx)
        {
            _ctx = ctx;
        }

        public IQueryable<UserUsageLog>? GetDailyLogs(string userId, DateOnly dayDate)
        {
            var dailyLogs = _ctx.DailyUsageLogs
                .Where(d => d.UserId == userId && d.DailyLogDate == dayDate)
                .Include(d => d.App);

            return dailyLogs;
        }

        // بترجع ليست فيها استخدام اليوزر علي مدار ريبنج معين
        public IQueryable<UserUsageLog>? GetLogsInRange(string userId, DateOnly startOfRange, DateOnly endOfRange)
        {
            var dailylogs = _ctx.DailyUsageLogs
                .Where(d => d.UserId == userId && d.DailyLogDate >= startOfRange && d.DailyLogDate <= endOfRange)
                .Include(d => d.App);

            return dailylogs;
        }

        public async Task<UserUsageLog?> GetLogByUserAppDate(string userId, int appId, DateOnly logDate)
        {
            var log = await _ctx.DailyUsageLogs
                .FirstOrDefaultAsync(d => d.UserId == userId && d.AppId == appId && d.DailyLogDate == logDate);
            return log;
        }
    }
}
