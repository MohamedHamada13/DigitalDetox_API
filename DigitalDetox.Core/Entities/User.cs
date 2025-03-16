using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDetox.Core.Entities
{
    internal class User
    {
       
        public int Id { get; set; }
        public string Name { get; set; }
        [DataType(DataType.EmailAddress) , Required]
        public string Email { get; set; }
        [DataType(DataType.Password), Required]
        public string PasswordHash { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<DetoxPlan> DetoxPlans { get; set; } = new HashSet<DetoxPlan>(); // done
        public ICollection<ScreenTimeLog> ScreenTimeLogs { get; set; } = new HashSet<ScreenTimeLog>(); // done
        public ICollection<ProgressLog>  ProgressLogs { get; set; } = new HashSet<ProgressLog>(); // done
        public UsersChallenges ChallengesForUser { get; set; }


    }
}
