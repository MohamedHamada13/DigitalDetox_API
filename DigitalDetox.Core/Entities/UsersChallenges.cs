using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDetox.Core.Entities
{
    public class UsersChallenges
    {
        public int? UserId { get; set; }
        public int? ChallengeId { get; set; }

        public User? User { get; set; } = default!;
        public Challenge? Challenge { get; set; } = default!;
    }
}
