using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDetox.Core.Entities
{
    internal class ScreenTimeLog
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public DateTime RecordedAt { get; set; }
        public int ScreenTimeMinutes { get; set; }
        public string AppUsed { get; set; }
        // navegation 
        public User User { get; set; }
    }
}
