using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDetox.Core.Entities.Models
{
    public class DailyUsageLog
    {
        public int Id { get; set; }
        public TimeSpan UsageTime { get; set; }
        public DateTime DailyLogDate { get; set; } // Should store only the date (no time) if per-day log [DateTime.Date]

        public int AppId { get; set; }
        public App App { get; set; } = default!;

        public string UserId { get; set; } // FK
        public AppUser User { get; set; } = default!; // NP
    }
}
