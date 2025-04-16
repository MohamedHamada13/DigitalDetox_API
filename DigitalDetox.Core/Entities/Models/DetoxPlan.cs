using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDetox.Core.Entities.Models
{
    public class DetoxPlan
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CreatedAt { get; set; } // For Creator(Admin) not user 

        public string? UserId { get; set; } // FK
        public AppUser? User { get; set; } // NP
    }
}
