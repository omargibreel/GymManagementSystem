using GymManagement.DAL.Data.Context;
using GymManagement.DAL.Entities;
using GymManagement.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Repositories.Classes
{
    internal class TrainerRepository : GenericRepository<Trainer>, ITrainerRepository
    {
        public TrainerRepository(GymDbContext context) : base(context)
        {

        }
    }
}
