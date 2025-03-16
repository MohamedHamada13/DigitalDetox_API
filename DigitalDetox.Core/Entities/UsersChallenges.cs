using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDetox.Core.Entities
{
    internal class UsersChallenges
    {
        // one to many user
        // one to many challenge
        public int UserId { get; set; }
        public int ChallengeId { get; set; }

        public ICollection<User> Users { get; set; } = new HashSet<User>();
        public ICollection<Challenge> Challenges { get; set; } = new HashSet<Challenge>();
    }
}
