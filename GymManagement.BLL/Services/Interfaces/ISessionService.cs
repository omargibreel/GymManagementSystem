using GymManagement.BLL.ViewModels.SessionViewModels;
using GymManagementSystemBLL.ViewModels.SessionViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Interfaces
{
    public interface ISessionService
    {
        IEnumerable<SessionViewModel> GetAllSessions();
        SessionViewModel? GetSessionById(int id);  
        bool CreateSession(CreateSessionViewModel createdSession);
        UpdateSessionViewModel? GetSessionForUpdate(int id);
        bool UpdateSession(int id, UpdateSessionViewModel updatedSession);
        bool RemoveSession(int id);

    }
}
