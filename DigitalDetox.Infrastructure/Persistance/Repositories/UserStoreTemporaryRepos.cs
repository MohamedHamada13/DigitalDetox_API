using DigitalDetox.Core.Context;
using DigitalDetox.Core.Entities.Models;
using DigitalDetox.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDetox.Infrastructure.Persistance.Repositories
{
    public class UserStoreTemporaryRepos : IUserStoreTemporaryRepos
    {
        private readonly DegitalDbContext _ctx;
        public UserStoreTemporaryRepos(DegitalDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<UserStoreTemporary?> GetByEmail(string email)
        {
            return await _ctx.UserStoreTemporary.FirstOrDefaultAsync(user => user.Email == email);
        }

        public async Task AddAsync(UserStoreTemporary model)
        {
            await _ctx.UserStoreTemporary.AddAsync(model);
            await SaveAsync();
        }

        public async Task RemoveAsync(UserStoreTemporary model)
        {
            _ctx.UserStoreTemporary.Remove(model);
            await SaveAsync();
        }
        
        public async Task UpdateAsync(UserStoreTemporary model)
        {
            _ctx.UserStoreTemporary.Update(model);
            await SaveAsync();
        }
        
        public async Task SaveAsync()
        {
            await _ctx.SaveChangesAsync();
        }
    }
}
