using DigitalDetox.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDetox.Core.Interfaces
{
    public interface IGenericRepos<T> where T : class
    {
        IQueryable<T>? GetAll();
        Task<T?> GetAsync(int id);
        Task AddAsync(T entity);
        void UpdateAsync(T entity);
        void DeleteAsync(T entity);
        Task SaveAsync();
    }
}

