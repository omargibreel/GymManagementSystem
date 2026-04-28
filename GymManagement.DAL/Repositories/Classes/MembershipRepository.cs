using GymManagement.DAL.Data.Context;
using GymManagement.DAL.Entities;
using GymManagement.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Repositories.Classes
{
    public class MembershipRepository : GenericRepository<Membership>, IMembershipRepository
    {
        private readonly GymDbContext _context;

        public MembershipRepository(GymDbContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<Membership> GetAllMembershipWithMembersAndPlans(Func<Membership, bool>? filter = null)
        {
            var memberships = _context.Memberships.Include(m => m.Member).Include(m => m.Plan)
                .Where(filter ?? (_ => true));
            return memberships;
        }
    }
}
