using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Play.Common.Entities;

namespace Play.Common.MongoDB
{
    public interface IRepository<T> where T : IEntity
    {
        Task CreateAsync(T entity);
        Task<IReadOnlyCollection<T>> GetAllAsync();
        Task<T> GetAsync(Guid id);
        Task RemoveAsync(Guid id);
        Task UpdateAsync(T entity);
    }
}