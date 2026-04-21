using GymManagement.DAL.Data.Context;
using GymManagement.DAL.Entities;
using GymManagement.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Repositories.Classes
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity, new()
    {
        private readonly GymDbContext _context;

        public GenericRepository(GymDbContext context)
        {
            _context = context;
        }

        public IEnumerable<TEntity> GetAll(Func<TEntity, bool>? condition = null)
        {
            if (condition is null)
                return _context.Set<TEntity>().AsNoTracking().ToList();
            var entities = _context.Set<TEntity>().AsNoTracking().Where(condition).ToList();
            return entities;
        }

        public TEntity? GetById(int id)
        {
            var entity = _context.Set<TEntity>().Find(id);
            return entity;
        }

        public void Add(TEntity entity) => _context.Set<TEntity>().Add(entity);


        public void Delete(TEntity entity) => _context.Set<TEntity>().Remove(entity);

        public void Update(TEntity entity) => _context.Set<TEntity>().Update(entity);
    }
}
