using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDetox.Core.DTOs.DailyUsageLogDtos
{
    public class DailyLogGetRequest
    {
        [Required]
        public string userId { get; set; }
        [Required]
        public DateOnly dayDate { get; set; }
    }
}
