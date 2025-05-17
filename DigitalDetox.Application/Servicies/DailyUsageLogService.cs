using DigitalDetox.Core.DTOs.DailyUsageLogDtos;
using DigitalDetox.Core.Entities.Models;
using DigitalDetox.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDetox.Application.Servicies
{
    public class DailyUsageLogService : IDailyUsageLogService
    {
        private readonly IHttpContextAccessor _httpContextAccessor; // To get the user details from the token
        private readonly IDailyUsageLogRepos _DailyRepos;
        private readonly IAppRepos _appRepos;
        private readonly UserManager<AppUser> _userManager;

        public DailyUsageLogService(IDailyUsageLogRepos Repos, IAppRepos appRepos, UserManager<AppUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _DailyRepos = Repos;
            _appRepos = appRepos;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }



        public async Task<DailyLogResponse> LogUsageAsync(DailyLogRequest model)
        {
            // Get the user id from the token (search manually for "uid")
            var userId = _httpContextAccessor.HttpContext?.User.Claims
                .FirstOrDefault(c => c.Type == "uid")?.Value;

            if (string.IsNullOrEmpty(userId))
                return new DailyLogResponse { Message = "Invalid token" };

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new DailyLogResponse { Message = "Invalid user id, user not found!" };
            }

            foreach (var appInfo in model.AppsInfo)
            {
                var appName = appInfo.AppName.Trim().ToLower();
                var app = await _appRepos.GetAppByNameAsync(appName);

                if (app == null)
                {
                    app = await _appRepos.AddNewApp(appName, appInfo.WebIconUrl);
                }

                try
                {
                    var dailyExistingLog = await _DailyRepos.GetLogByUserAppDate(userId, app.Id, model.LogDate);
                    if (dailyExistingLog == null)
                    {
                        await _DailyRepos.AddAsync(new UserUsageLog
                        {
                            UserId = userId,
                            AppId = app.Id,
                            UsageTime = appInfo.TimeUsage,
                            DailyLogDate = model.LogDate,
                        });
                    }
                    else
                    {
                        dailyExistingLog.UsageTime += appInfo.TimeUsage;
                        await _DailyRepos.UpdateAsync(dailyExistingLog);
                    }
                }
                catch (Exception ex)
                {
                    return new DailyLogResponse { Message = $"Failed to log usage for app '{appInfo.AppName}': {ex.Message}" };
                }
            }
            return new DailyLogResponse { Message = "Log process is succeeded", IsLogged = true };
        }

        public async Task<DailyUsageLogGetDto?> GetDailyLogsAsync(DateOnly dayDate)
        {
            var userId = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;
            if (string.IsNullOrEmpty(userId))
                return null;

            var userName = await _userManager.Users
                .Where(u => u.Id == userId)
                .Select(u => $"{u.FirstName} {u.LastName}")
                .FirstOrDefaultAsync();

            if (userName == null)
                return null;

            var userDailyLogs = await _DailyRepos.GetDailyLogs(userId, dayDate)!.ToListAsync();
            if (!userDailyLogs.Any())
                return new DailyUsageLogGetDto
                {
                    userName = userName,
                    dayDate = dayDate,
                    Logs = []
                };

            var subUserDailyLogsDto = userDailyLogs.Select(log => new SubDailyUsageLogGetDto(log)).ToList();

            var userDailyLogsDto = new DailyUsageLogGetDto
            {
                userName = userName,
                dayDate = dayDate,
                Logs = subUserDailyLogsDto
            };

            return userDailyLogsDto;
        }

        public async Task<InRangeUsageLogGetDto?> GetLogsInRangeAsync(UsageInRangeRequest model)
        {
            var userId = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;
            if (string.IsNullOrEmpty(userId))
                return null;

            var user = await _userManager.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
                return null;

            var userInRangeLogs = await _DailyRepos.GetLogsInRange(userId, model.StartDate, model.EndDate)
                .ToListAsync();

            var logsGroupedByDate = userInRangeLogs
                .GroupBy(log => log.DailyLogDate)
                .OrderBy(g => g.Key)
                .ToList();

            var userInRangeUsage = new InRangeUsageLogGetDto
            {
                userName = $"{user.FirstName} {user.LastName}",
                Logs = new List<SubInRangeUsageLogGetDto>()
            };

            var totalDays = (model.EndDate.ToDateTime(TimeOnly.MinValue) - model.StartDate.ToDateTime(TimeOnly.MinValue)).Days + 1;

            for (int i = 0; i < totalDays; i++)
            {
                var currentDate = model.StartDate.AddDays(i);

                var group = logsGroupedByDate.FirstOrDefault(g => g.Key == currentDate);

                userInRangeUsage.Logs.Add(new SubInRangeUsageLogGetDto
                {
                    DayDate = currentDate,
                    DailyLogs = group?.Select(log => new SubDailyUsageLogGetDto(log)).ToList() ?? new List<SubDailyUsageLogGetDto>()
                });
            }
        
            return userInRangeUsage;
        }


        public Task<IEnumerable<UserUsageLog>> GetMonthlyLogsAsync(string userId, int month, int year)
        {
            throw new NotImplementedException();
        }

        public Task<TimeSpan> GetTotalUsageAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UserUsageLog>> GetWeeklyLogsAsync(string userId, DateTime startOfWeek)
        {
            throw new NotImplementedException();
        }


    }
}
