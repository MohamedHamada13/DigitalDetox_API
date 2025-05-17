using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDetox.Core.DTOs.DailyUsageLogDtos
{
    public class InRangeUsageLogGetDto
    {
        public string userName { get; set; }
        public List<SubInRangeUsageLogGetDto> Logs { get; set; }
    }
}
