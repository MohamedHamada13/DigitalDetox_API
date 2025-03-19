using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDetox.Core.Entities
{
    public class User
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string PasswordHash { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public DateTime CreatedAt { get; set; }

        public ICollection<DetoxPlan>? DetoxPlans { get; set; }
        public ICollection<ScreenTimeLog>? ScreenTimeLogs { get; set; }
        public ICollection<ProgressLog>? ProgressLogs { get; set; }
        public ICollection<UsersChallenges>? UserChallenges { get; set; } // NP
    }
}
