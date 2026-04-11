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
    public class SessionRepository : GenericRepository<Session>, ISessionRepository
    {
        private readonly GymDbContext _context;

        public SessionRepository(GymDbContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<Session> GetAllSessionsWithTrainerAndCategory()
        {
            return _context.Sessions.Include(s => s.SessionTrainer)
                                            .Include(s => s.SessionCategory)
                                            .ToList();
        }

        public int GetCountOfBookingSlots(int sessionId)
        {
            return _context.MemberSessions.Count(ms => ms.SessionId == sessionId);  
        }

        public Session? GetSessionWithTrainerAndCategory(int id)
        {
            return _context.Sessions.Include(s => s.SessionTrainer)
                                            .Include(s => s.SessionCategory)
                                            .FirstOrDefault(s => s.Id == id);
        }
    } 
}
