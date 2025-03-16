using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDetox.Core.Entities
{
    internal class ProgressLog
    {
        /*
         - Id (int, Primary Key)
        - UserId (int, Foreign Key referencing User)
        - RecordedAt (DateTime, required)
        - ProgressDescription (string, required)
        - User (Navigation Property, Many-to-One with User)
         */

        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime RecordedAt { get; set; }
        public string ProgressDescription { get; set; }
        // navegation 
        public User User { get; set; }

    }
}
