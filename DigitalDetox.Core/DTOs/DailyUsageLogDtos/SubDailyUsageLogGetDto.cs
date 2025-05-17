using DigitalDetox.Core.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDetox.Core.DTOs.DailyUsageLogDtos
{
    public class SubDailyUsageLogGetDto
    {
        public string AppName { get; set; }
        public TimeSpan UsageTime { get; set; }
        public string? WebIconUrl { get; set; }



        public SubDailyUsageLogGetDto() { }

        public SubDailyUsageLogGetDto(UserUsageLog model)
        {
            UsageTime = model.UsageTime;
            AppName = model.App.Name;
            WebIconUrl = model.App.WebIconUrl;
        }
    }
}
