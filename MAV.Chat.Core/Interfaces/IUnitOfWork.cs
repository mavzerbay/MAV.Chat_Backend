using MAV.Chat.Core.Entities;
using System;
using System.Threading.Tasks;

namespace MAV.Chat.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<TEntity> Repository<TEntity>();
        Task<int> SaveChangesAsync();
        bool HasChanges();
    }
}
