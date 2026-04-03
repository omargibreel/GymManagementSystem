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
    internal class SessionRepository : GenericRepository<Session>, ISessionRepository
    {
        public SessionRepository(GymDbContext context) : base(context)
        {

        }
    }
}
