using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDetox.Core.DTOs.DailyUsageLogDtos
{
    public class AppUsageInfo {
        [Required]
        public string AppName { get; set; }
        [Required]
        public TimeSpan TimeUsage { get; set; }
        public string? WebIconUrl { get; set; }
    }
}
