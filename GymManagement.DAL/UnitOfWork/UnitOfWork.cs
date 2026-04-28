using GymManagement.DAL.Data.Context;
using GymManagement.DAL.Entities;
using GymManagement.DAL.Repositories.Classes;
using GymManagement.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        // i don't need to implement IDisposable because the context will be disposed by the DI container, and the repositories don't hold any unmanaged resources that require explicit disposal.
        // the clr will automatically clean up the repositories when the UnitOfWork instance is garbage collected, so there's no need for manual disposal logic in this case.

        private readonly Dictionary<Type, object> _repositories = new();
        private readonly GymDbContext _context;

        public UnitOfWork(GymDbContext context, ISessionRepository sessionRepository, IMembershipRepository membershipRepository)
        {
            _context = context;
            SessionRepository = sessionRepository;
            MembershipRepository = membershipRepository;
        }

        public ISessionRepository SessionRepository { get; }

        public IMembershipRepository MembershipRepository { get; }

        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new()
        {
            var entityType = typeof(TEntity);
            if (_repositories.TryGetValue(entityType, out var repo))
                return (IGenericRepository<TEntity>)repo;

            var newRepo = new GenericRepository<TEntity>(_context);
            _repositories[entityType] = newRepo;
            return newRepo;
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }
    }
}
