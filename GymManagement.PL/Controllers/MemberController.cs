using Microsoft.AspNetCore.Mvc;

namespace GymManagement.PL.Controllers
{
    public class MemberController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public ActionResult GetMembers()
        {
            return View();
        }
        public ActionResult CreateMember() 
        {
            return View(); 
        }
    }
}
