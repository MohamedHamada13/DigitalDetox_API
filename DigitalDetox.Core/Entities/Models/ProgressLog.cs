using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDetox.Core.Entities.Models
{
    public class ProgressLog
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public DateTime RecordedAt { get; set; }
        public required string ProgressDescription { get; set; }

        public AppUser? User { get; set; } // NP

    }
}
