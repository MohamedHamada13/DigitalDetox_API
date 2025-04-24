using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDetox.Core.DTOs.DailyUsageLogDtos
{
    public class DailyUsageLogGetDto
    {
        public string userName { get; set; }
        public DateOnly dayDate { get; set; }
        public List<SubDailyUsageLogGetDto> Logs { get; set; }
    }
}
