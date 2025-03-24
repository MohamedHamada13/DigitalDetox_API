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
        IQueryable<T>? GetChallenges();
        Task<T?> GetChallengeAsync(int id);
        Task AddChallengeAsync(T entity);
        void UpdateChallengeAsync(T entity);
        void DeleteChallengeAsync(T entity);
        Task SaveAsync();
    }
}

