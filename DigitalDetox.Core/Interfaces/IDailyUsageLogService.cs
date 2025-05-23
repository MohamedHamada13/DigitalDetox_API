﻿using DigitalDetox.Core.DTOs.DailyUsageLogDtos;
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
        Task<DailyLogResponse> LogUsageAsync(DailyLogRequest model);


        Task<DailyUsageLogGetDto?> GetDailyLogsAsync(DateOnly dayDate);
        Task<InRangeUsageLogGetDto?> GetLogsInRangeAsync(UsageInRangeRequest model);

        Task<IEnumerable<UserUsageLog>> GetMonthlyLogsAsync(string userId, int month, int year);

        Task<TimeSpan> GetTotalUsageAsync(string userId);


    }
}
