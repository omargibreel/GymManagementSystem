using GymManagement.BLL.Services.Classes;
using GymManagement.BLL.Services.Interfaces;
using GymManagementSystemBLL.ViewModels.SessionViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagement.PL.Controllers
{
    [Authorize]
    public class SessionController : Controller
    {
        private readonly ISessionService _sessionService;
        public SessionController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }
        public IActionResult Index()
        {
            var sessions = _sessionService.GetAllSessions();
            return View(sessions);
        }
        public IActionResult Details(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid session ID.";
                return RedirectToAction(nameof(Index));
            }
            var session = _sessionService.GetSessionById(id);
            if (session == null)
            {
                TempData["ErrorMessage"] = "Session Not Found.";
                return RedirectToAction(nameof(Index));
            }
            return View(session);
        }

        public IActionResult Create()
        {
            LoadDropDownsForCategories();
            LoadDropDownsForTrainers();
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateSessionViewModel createdSession)
        {
            if (!ModelState.IsValid)
            {
                LoadDropDownsForCategories();
                LoadDropDownsForTrainers();
                return View(createdSession);
            }
            var result = _sessionService.CreateSession(createdSession);
            if (result)
            {
                TempData["SuccessMessage"] = "Session created successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to create session.";
                LoadDropDownsForCategories();
                LoadDropDownsForTrainers();
                return View(createdSession);
            }
            return RedirectToAction(nameof(Index));
        }

        public ActionResult Edit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid session ID.";
                return RedirectToAction(nameof(Index));
            }
            var session = _sessionService.GetSessionForUpdate(id);
            if (session == null)
            {
                TempData["ErrorMessage"] = "Session Can Not Be Updating.";
                return RedirectToAction(nameof(Index));
            }
            LoadDropDownsForTrainers();
            return View(session);
        }
        [HttpPost]
        public ActionResult Edit(int id, UpdateSessionViewModel updatedSession)
        {
            if (!ModelState.IsValid)
            {
                LoadDropDownsForTrainers();
                return View(updatedSession);
            }
            var result = _sessionService.UpdateSession(id, updatedSession);
            if (result)
            {
                TempData["SuccessMessage"] = "Session updated successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to update session.";
                LoadDropDownsForTrainers();
                return View(updatedSession);
            }
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Delete(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid session ID.";
                return RedirectToAction(nameof(Index));
            }
            var session = _sessionService.GetSessionById(id);
            if (session is null)
            {
                TempData["ErrorMessage"] = "Session Not Found.";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.SessionId = session.Id;
            return View();
        }
        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            var result = _sessionService.RemoveSession(id);
            if (result)
            {
                TempData["SuccessMessage"] = "Session Deleted Successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed To Delete Session.";
            }
            return RedirectToAction(nameof(Index));
        }

        #region HelperMethods
        private void LoadDropDownsForCategories()
        {
            var categories = _sessionService.GetCategoryForDropDown();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");        }
        private void LoadDropDownsForTrainers()
        {
            var trainers = _sessionService.GetTrainerForDropDown();
            ViewBag.Trainers = new SelectList(trainers, "Id", "Name");
        }
        #endregion

    }
}
