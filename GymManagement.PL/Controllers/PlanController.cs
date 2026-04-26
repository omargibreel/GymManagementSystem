using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.PlanViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Update.Internal;

namespace GymManagement.PL.Controllers
{
    [Authorize]
    public class PlanController : Controller
    {
        private readonly IPlanService _planService;

        public PlanController(IPlanService planService)
        {
            _planService = planService;
        }

        public IActionResult Index()
        {
            var plans = _planService.GetAllPlans();
            return View(plans);
        }
        public IActionResult Details(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid plan ID.";
                return RedirectToAction(nameof(Index));
            }
            var plan = _planService.GetPlanById(id);
            if (plan == null)
            {
                TempData["ErrorMessage"] = "Plan not found.";
                return RedirectToAction(nameof(Index));
            }
            return View(plan);
        }
        public IActionResult Edit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid plan ID.";
                return RedirectToAction(nameof(Index));
            }
            var plan = _planService.GetPlanById(id);
            if (plan == null)
            {
                TempData["ErrorMessage"] = "Plan Not Found.";
                return RedirectToAction(nameof(Index));
            }
            var planToUpdate = _planService.GetPlanToUpdate(id);
            if (planToUpdate == null)
            {
                TempData["ErrorMessage"] = "Plan Can Not Be Updated.";
                return RedirectToAction(nameof(Index));
            }
            return View(planToUpdate);
        }
        [HttpPost]
        public IActionResult Edit([FromRoute] int id, PlanToUpdateViewModel updatedPlan)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("WrongData", "Check Data Validation");
                return View(updatedPlan);
            }
            var result = _planService.UpdatePlan(id, updatedPlan);
            if (result)
            {
                TempData["SuccessMessage"] = "Plan updated successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Plan Failed To Update";
            }
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Activate(int id)
        {
            var result = _planService.ToggleStatus(id);
            if (result)
            {
                TempData["SuccessMessage"] = "Plan status updated successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to update plan status.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
