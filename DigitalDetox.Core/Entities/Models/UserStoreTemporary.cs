using DigitalDetox.Core.Entities.AuthModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDetox.Core.Entities.Models
{
    // is a tempClass to store the user data untill verifing the user email
    public class UserStoreTemporary
    {
        [Required, MaxLength(80)]
        public string? FirstName { get; set; }

        [Required, MaxLength(80)]
        public string? LastName { get; set; }

        [Required, MaxLength(100)]
        public string? UserName { get; set; }

        [Required, MaxLength(128)]
        public string? Email { get; set; }

        [Required, MaxLength(256)]
        public string? Password { get; set; }

        [Required]
        public DateOnly? DateOfBirth { get; set; }
        [Required]
        public string? Code { get; set; }
        [Required]
        public DateTime? ExpiresAt { get; set; }


        public UserStoreTemporary() { }

        public UserStoreTemporary(SignUpReqModel model)
        {
            FirstName = model.FirstName;
            LastName = model.LastName;
            UserName = model.UserName;
            Email = model.Email;
            Password = model.Password;
            DateOfBirth = model.DateOfBirth;
            Code = GetRanCode();
            ExpiresAt = DateTime.UtcNow.AddMinutes(10);
        }

        // Generate a random number of 6 digits
        private string GetRanCode()
        {
            var code = new Random().Next(100000, 999999).ToString();
            return code;
        }

    }
}
