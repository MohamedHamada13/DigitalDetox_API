using DigitalDetox.Core.DTOs.UserDtos;
using DigitalDetox.Core.Entities.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DigitalDetox.Core.Interfaces;


namespace DigitalDetox.Application.Servicies
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        public UserService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IEnumerable<userGetDto>> GetAllUsers()
        {
            var query = _userManager.Users.Select(u => new userGetDto(u));
            var users = await (query.ToListAsync());

            return users;
        }

        
    }
}
