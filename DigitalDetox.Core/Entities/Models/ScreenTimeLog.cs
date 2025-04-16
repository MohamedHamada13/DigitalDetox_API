using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDetox.Core.Entities.Models
{
    public class ScreenTimeLog
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public DateTime RecordedAt { get; set; }
        public int ScreenTimeMinutes { get; set; }
        public required string AppUsed { get; set; }

        public AppUser? User { get; set; } = default!;
    }
}
