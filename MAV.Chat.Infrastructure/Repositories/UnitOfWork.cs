using MAV.Chat.Core.Entities;
using MAV.Chat.Core.Interfaces;
using MAV.Chat.Infrastructure.Context;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAV.Chat.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MavDbContext _context;
        private Hashtable _repositories;
        public UnitOfWork(MavDbContext context)
        {
            _context = context;

        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public IGenericRepository<TEntity> Repository<TEntity>()
        {
            if (_repositories == null) _repositories = new Hashtable();

            var type = typeof(TEntity).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(GenericRepository<>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _context);

                _repositories.Add(type, repositoryInstance);
            }

            return (IGenericRepository<TEntity>)_repositories[type];
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
        public bool HasChanges()
        {
            return _context.ChangeTracker.HasChanges();
        }
    }
}
