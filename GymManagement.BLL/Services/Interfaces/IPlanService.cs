using GymManagement.BLL.ViewModels.PlanViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Interfaces
{
    public interface IPlanService
    {
        IEnumerable<PlanViewModel> GetAllPlans();
        PlanViewModel? GetPlanById(int id);
        PlanToUpdateViewModel? GetPlanToUpdate(int id);
        bool UpdatePlan(int id, PlanToUpdateViewModel plan);
        bool ToggleStatus(int id);
    }
}
