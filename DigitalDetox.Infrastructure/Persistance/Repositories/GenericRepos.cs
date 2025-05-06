using DigitalDetox.Core.Context;
using DigitalDetox.Core.Enums;
using DigitalDetox.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

        public IQueryable<T>? GetAll()
        {
            return _dbSet.AsQueryable();
        }

        public async Task<T?> GetAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await SaveAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            await SaveAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await SaveAsync();
        }

        public async Task SaveAsync()
        {
            await _ctx.SaveChangesAsync();
        }
    }
}
