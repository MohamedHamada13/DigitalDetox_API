using DigitalDetox.Core.DTOs.DailyUsageLogDtos;
using DigitalDetox.Core.Entities.Models;
using DigitalDetox.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDetox.Application.Servicies
{
    public class DailyUsageLogService : IDailyUsageLogService
    {
        private readonly IDailyUsageLogRepos _DailyRepos;
        private readonly IAppRepos _appRepos;
        private readonly UserManager<AppUser> _userManager;

        public DailyUsageLogService(IDailyUsageLogRepos Repos, IAppRepos appRepos, UserManager<AppUser> userManager)
        {
            _DailyRepos = Repos;
            _appRepos = appRepos;
            _userManager = userManager;
        }

        
        public async Task<DailyLogResponse> LogUsageAsync(DailyLogRequest model)
        {
            foreach (var map in model.appUsages) // map.Key -> name of the app
            {
                var appName = map.Key.Trim().ToLower();
                var app = await _appRepos.IsExist(appName);

                if(app == null) // if app is not exist, create it.
                {
                    app = await _appRepos.AddNewApp(appName);
                }

                try
                {
                    var dailyExistingLog = await _DailyRepos.GetLogByUserAppDate(model.userId, app.Id, model.logDate);
                    if(dailyExistingLog == null) // means it is the first time to make log in this day
                    {
                        await _DailyRepos.AddAsync(new UserUsageLog
                        {
                            UserId = model.userId,
                            AppId = app.Id,
                            UsageTime = map.Value,
                            DailyLogDate = model.logDate, // Date make the format -> "2025-04-18"
                        });
                    }
                    else
                    {
                        dailyExistingLog.UsageTime += map.Value;
                        _DailyRepos.UpdateAsync(dailyExistingLog);
                    }
                    await _DailyRepos.SaveAsync();
                }
                catch(Exception ex)
                {
                    return new DailyLogResponse { Message = $"Failed to log usage for app '{map.Key}': {ex.Message}" };
                }
                
            }
            return new DailyLogResponse { Message = "Log process is succeded", IsLoged = true };
        }

        public async Task<DailyUsageLogGetDto?> GetDailyLogsAsync(string userId, DateOnly dayDate)
        {
            var userDailyLogs = await _DailyRepos.GetDailyLogs(userId, dayDate)!.ToListAsync();
            if(userDailyLogs == null)
                return null;

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
                return null;

            var subUserDailyLogsDto = userDailyLogs.Select(log => new SubDailyUsageLogGetDto(log)).ToList();

            var userDailyLogsDto = new DailyUsageLogGetDto
            {
                userName = $"{user?.FirstName} {user?.LastName}",
                dayDate = dayDate,
                Logs = subUserDailyLogsDto
            };;

            return userDailyLogsDto;
        }

        public async Task<WeeklyUsageLogGetDto?> GetLogsInRangeAsync(string userId, DateOnly startDate, DateOnly endDate)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
                return null;

            var userWeekLogs = await _DailyRepos.GetLogsInRange(userId, startDate, endDate)
                .ToListAsync();

            var logsGrouped = userWeekLogs
                .GroupBy(log => log.DailyLogDate)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(log => new SubDailyUsageLogGetDto(log)).ToList()
                );

            var userWeekUsage = new WeeklyUsageLogGetDto
            {
                userName = $"{user.FirstName} {user.LastName}",
                weekLogs = new List<SubWeeklyUsageLogGetDto>()
            };

            var totalDays = (endDate.ToDateTime(TimeOnly.MinValue) - startDate.ToDateTime(TimeOnly.MinValue)).Days + 1;

            for (int i = 0; i < totalDays; ++i)
            {
                var currentDate = startDate.AddDays(i);
                logsGrouped.TryGetValue(currentDate, out var dailyLogs);

                userWeekUsage.weekLogs.Add(new SubWeeklyUsageLogGetDto
                {
                    DayDate = currentDate,
                    DailyLogs = dailyLogs ?? new List<SubDailyUsageLogGetDto>()
                });
            }

            return userWeekUsage;
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
