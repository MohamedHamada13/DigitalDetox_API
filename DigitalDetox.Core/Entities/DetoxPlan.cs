using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDetox.Core.Entities
{
    internal class DetoxPlan
    {
   
        public int Id { get; set; }
        public int? UserId { get; set; }
        public string Title { get; set; } 
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime CreatedAt { get; set; }
        // navegation 
        public User User { get; set; } 

    }
}
