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
    internal class MemberRepository : GenericRepository<Member>,IMemberRepository
    {
        // ask clr to inject the context for us and pass it to the base class constructor
        public MemberRepository(GymDbContext context):base(context)
        {
        }
    }
}
