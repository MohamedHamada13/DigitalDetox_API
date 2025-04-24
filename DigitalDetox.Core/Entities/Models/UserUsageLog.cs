using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDetox.Core.Entities.Models
{
    public class UserUsageLog
    {
        public int Id { get; set; }
        public TimeSpan UsageTime { get; set; }
        public DateOnly DailyLogDate { get; set; } 

        public int AppId { get; set; }
        public App App { get; set; } = default!;

        public string UserId { get; set; } // FK
        public AppUser User { get; set; } = default!; // NP
    }
}
