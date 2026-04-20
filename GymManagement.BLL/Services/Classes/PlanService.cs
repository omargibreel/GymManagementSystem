using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.PlanViewModels;
using GymManagement.DAL.Entities;
using GymManagement.DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Classes
{
    public class PlanService : IPlanService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PlanService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IEnumerable<PlanViewModel> GetAllPlans()
        {
            var plans = _unitOfWork.GetRepository<Plan>().GetAll();
            if (plans is null || !plans.Any())
                return [];

            return plans.Select(p => new PlanViewModel()
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Description = p.Description,
                DurationDays = p.DurationDays,
                IsActive = p.IsActive
            });
        }


        public PlanViewModel? GetPlanById(int id)
        {
            var plan = _unitOfWork.GetRepository<Plan>().GetById(id);
            if (plan is null)
                return null;
            return new PlanViewModel()
            {
                Id = plan.Id,
                Name = plan.Name,
                Price = plan.Price,
                Description = plan.Description,
                DurationDays = plan.DurationDays,
                IsActive = plan.IsActive
            };
        }

        public PlanToUpdateViewModel? GetPlanToUpdate(int id)
        {
            var plan = _unitOfWork.GetRepository<Plan>().GetById(id);
            if (plan is null || plan.IsActive == false || HasActiveMemberships(id))
                return null;
            return new PlanToUpdateViewModel()
            {
                PlanName = plan.Name,
                Description = plan.Description,
                Price = plan.Price,
                DurationDays = plan.DurationDays
            };
        }
        public bool UpdatePlan(int id, PlanToUpdateViewModel newPlan)
        {
            try
            {
                var plan = _unitOfWork.GetRepository<Plan>().GetById(id);
                if (plan is null || HasActiveMemberships(id))
                    return false;

                (plan.Description, plan.Price, plan.UpdatedAt, plan.DurationDays)
                    = (newPlan.Description, newPlan.Price, DateTime.UtcNow, plan.DurationDays);
                _unitOfWork.GetRepository<Plan>().Update(plan);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }

        public bool ToggleStatus(int id)
        {
            var plan = _unitOfWork.GetRepository<Plan>().GetById(id);
            if (plan is null || HasActiveMemberships(id))
                return false;

            plan.IsActive = plan.IsActive == true ? false : true;
            plan.UpdatedAt = DateTime.UtcNow;
            return _unitOfWork.SaveChanges() > 0;
        }


        private bool HasActiveMemberships(int id)
        {
            var activeMemberships = _unitOfWork.GetRepository<Membership>()
                .GetAll(m => m.PlanId == id && m.EndDate > DateTime.UtcNow);

            return activeMemberships.Any();
        }
    }
}
