using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDetox.Core.Entities.Models
{
    public class App
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<UserUsageLog>? DailyLogs { get; set; } = new List<UserUsageLog>();
    }
}
