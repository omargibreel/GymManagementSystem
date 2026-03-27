using GymManagement.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Repositories.Interfaces
{
    internal interface IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        // GetById
        Task<TEntity> GetByIdAsync(int id);

        // GetAll
        Task<IEnumerable<TEntity>> GetAllAsync();

        // Add
        void Add(TEntity entity);
        // Update
        // Delete
        void Delete(int id);

    }
}
