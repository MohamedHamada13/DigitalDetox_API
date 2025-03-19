using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDetox.Core.Entities
{
    public class ProgressLog
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public DateTime RecordedAt { get; set; }
        public required string ProgressDescription { get; set; } 
        
        public User? User { get; set; } // NP

    }
}
