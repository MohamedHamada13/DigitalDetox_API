using DigitalDetox.Core.DTOs.UserDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDetox.Core.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<userGetDto>> GetAllUsers();
    }
}
