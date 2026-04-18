using GymManagement.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.PL.Controllers
{
    public class Trainer : Controller
    {
        private readonly ITrainerService _trainerService;

        public Trainer(ITrainerService trainerService)
        {
             _trainerService = trainerService;
        }
        public IActionResult Index()
        {
            var trainers = _trainerService.GetAllTrainers();
            return View(trainers);
        }
    }
}
