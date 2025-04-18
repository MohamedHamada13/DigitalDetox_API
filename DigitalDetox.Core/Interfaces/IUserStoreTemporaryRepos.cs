using DigitalDetox.Core.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDetox.Core.Interfaces
{
    public interface IUserStoreTemporaryRepos
    {
        Task AddAsync(UserStoreTemporary model);
        Task SaveAsync();
        Task<UserStoreTemporary?> GetByEmail(string email);
        Task RemoveAsync(UserStoreTemporary model);
        Task UpdateAsync(UserStoreTemporary model);
    }
}
