using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.TrainerViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.PL.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class TrainerController : Controller
    {
        private readonly ITrainerService _trainerService;

        public TrainerController(ITrainerService trainerService)
        {
            _trainerService = trainerService;
        }
        public IActionResult Index()
        {
            var trainers = _trainerService.GetAllTrainers();
            return View(trainers);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(CreateTrainerViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("DataMissed", "Please fill in all required fields.");
                return View(nameof(Create), model);
            }

            var result = _trainerService.CreateTrainer(model);

            if (result)
                TempData["SuccessMessage"] = "Trainer created successfully.";
            else
                TempData["ErrorMessage"] = "Failed to create trainer. Please try again.";

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid trainer ID, Must be greater than zero.";
                return RedirectToAction(nameof(Index));
            }

            var trainer = _trainerService.GetTrainer(id);

            if (trainer is null)
            {
                TempData["ErrorMessage"] = "Trainer not found.";
                return RedirectToAction(nameof(Index));
            }
            return View(trainer);
        }

        public IActionResult Edit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid trainer ID, Must be greater than zero.";
                return RedirectToAction(nameof(Index));
            }
            var trainer = _trainerService.GetTrainerToUpdate(id);
            if (trainer is null)
            {
                TempData["ErrorMessage"] = "Trainer not found.";
                return RedirectToAction(nameof(Index));
            }
            return View(trainer);
        }

        [HttpPost]
        public IActionResult Edit([FromRoute] int id, UpdateTrainerViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("DataMissed", "Check Missing Fields");
                return View(model);
            }
            var result = _trainerService.UpdateTrainer(model, id);

            if (result)
                TempData["SuccessMessage"] = "Trainer updated successfully.";
            else
                TempData["ErrorMessage"] = "Failed to update trainer. Please try again.";

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid trainer ID, Must be greater than zero.";
                return RedirectToAction(nameof(Index));
            }

            var trainer = _trainerService.GetTrainer(id);

            if (trainer is null)
            {
                TempData["ErrorMessage"] = "Trainer not found.";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.TrainerId = id;
            ViewBag.TrainerName = trainer.Name;
            return View();
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            var result = _trainerService.RemoveTrainer(id);
            if (result)
                TempData["SuccessMessage"] = "Trainer deleted successfully.";
            else
                TempData["ErrorMessage"] = "Failed to delete trainer. Please try again.";
            return RedirectToAction(nameof(Index));
        }
    }
}