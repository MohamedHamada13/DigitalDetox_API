using DigitalDetox.Core.Context;
using DigitalDetox.Core.Enums;
using DigitalDetox.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDetox.Infrastructure.Persistance.Repositories
{
    public class GenericRepos<T> : IGenericRepos<T> where T : class
    {
        private readonly DegitalDbContext _ctx;
        private readonly DbSet<T> _dbSet;
        public GenericRepos(DegitalDbContext ctx)
        {
            _ctx = ctx;
            _dbSet = ctx.Set<T>();
        }

        public IQueryable<T>? GetChallenges()
        {
            return _dbSet.AsQueryable();
        }

        public async Task<T?> GetChallengeAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task AddChallengeAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void DeleteChallengeAsync(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void UpdateChallengeAsync(T entity)
        {
            _dbSet.Update(entity);
        }

        public async Task SaveAsync()
        {
            await _ctx.SaveChangesAsync();
        }
    }
}
