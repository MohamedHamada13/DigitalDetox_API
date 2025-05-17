using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDetox.Core.DTOs.DailyUsageLogDtos
{
    public class SubInRangeUsageLogGetDto
    {
        public DateOnly DayDate { get; set; }
        public List<SubDailyUsageLogGetDto> DailyLogs { get; set; }
    }
}
