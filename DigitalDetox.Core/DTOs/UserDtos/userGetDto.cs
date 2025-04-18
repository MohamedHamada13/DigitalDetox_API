using DigitalDetox.Core.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDetox.Core.DTOs.UserDtos
{
    public class userGetDto
    {
        public string? Id { get; set; }
        public string? FullName { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }

        public userGetDto() { }
        public userGetDto(AppUser user)
        {
            Id = user.Id;
            FullName = $"{user.FirstName} {user.LastName}";
            UserName = user.UserName;
            Email = user.Email;
        }
    }
}
