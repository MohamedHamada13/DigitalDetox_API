using DigitalDetox.Core.Entities.AuthModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDetox.Core.Entities.Models
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public DateTime CreatedAt { get; set; }

        public string? RefreshToken { get; set; } = null!;
        public DateTime RefreshTokenExpiryTime { get; set; }

        public ICollection<UserUsageLog>? DailyUsageLogs { get; set; }
        public ICollection<Challenge>? Challenges { get; set; }


        // Default Ctor
        public AppUser() { }

        // Map the `RegisterModel` into `AppUser`
        public AppUser(SignUpReqModel model)
        {
            FirstName = model.FirstName;
            LastName = model.LastName;
            DateOfBirth = model.DateOfBirth;
            CreatedAt = DateTime.Now;

            Email = model.Email;
            UserName = model.UserName;
        }

        public AppUser(UserStoreTemporary model)
        {
            FirstName = model.FirstName;
            LastName = model.LastName;
            DateOfBirth = model.DateOfBirth;
            CreatedAt = DateTime.UtcNow;

            Email = model.Email;
            UserName = model.UserName;
        }
    }
}
