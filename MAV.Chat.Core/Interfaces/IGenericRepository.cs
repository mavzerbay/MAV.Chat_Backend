using MAV.Chat.Core.Entities;
using MAV.Chat.Core.Specifications;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MAV.Chat.Core.Interfaces
{
    public interface IGenericRepository<T>
    {
        Task<T> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T> GetEntityWithSpec(ISpecification<T> spec);
        Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);
        Task<int> CountAsync(ISpecification<T> spec);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
