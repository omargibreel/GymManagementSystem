using GymManagement.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.PL.Controllers
{
    public class MembershipController(IMembershipServices _membershipService) : Controller
    {
        public IActionResult Index()
        {
            var memberships = _membershipService.GetAllMemberships();
            return View(memberships); 
        }
    }
}
