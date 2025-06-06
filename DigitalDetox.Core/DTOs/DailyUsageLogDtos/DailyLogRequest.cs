﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDetox.Core.DTOs.DailyUsageLogDtos
{
    public class DailyLogRequest
    {
        [Required]
        public DateOnly LogDate { get; set; }

        [Required]
        public List<AppUsageInfo> AppsInfo { get; set; }
    }
}
