using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.MemberViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.PL.Controllers
{
    public class MemberController : Controller
    {
        private readonly IMemberService _memberService;

        public MemberController(IMemberService memberService)
        {
            _memberService = memberService;
        }

        public IActionResult Index()
        {
            var members = _memberService.GetAllMembers();
            return View(members);
        }

        public ActionResult MemberDetails(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid member ID, Must be greater than zero.";
                return RedirectToAction(nameof(Index));
            }
            var member = _memberService.GetMemberDetails(id);
            if (member is null)
            {
                TempData["ErrorMessage"] = "Member not found.";
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }

        public ActionResult HealthRecordDetails(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid member ID, Must be greater than zero.";
                return RedirectToAction(nameof(Index));
            }
            var healthRecord = _memberService.GetMemberHealthRecord(id);
            if (healthRecord is null)
            {
                TempData["ErrorMessage"] = "Health record not found.";
                return RedirectToAction(nameof(Index));
            }

            return View(healthRecord);
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateMember(CreateMemberViewModel CreatedMember)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("DataInvalid", "Check Data And Missing Fields.");
                return View(nameof(Create), CreatedMember);
            }

            bool result = _memberService.CreateMember(CreatedMember);
            if (result)
            {
                TempData["SuccessMessage"] = "Member Created Successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to Create Member. Please Check Phone and Email.";
            }
            return RedirectToAction(nameof(Index));
        }

        public ActionResult Edit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid member ID, Must be greater than zero.";
                return RedirectToAction(nameof(Index));
            }
            var member = _memberService.GetMemberToUpdate(id);
            if (member is null)
            {
                TempData["ErrorMessage"] = "Member not found.";
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }
        [HttpPost]
        public ActionResult Edit(MemberToUpdateViewModel updatedMember, [FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return View(updatedMember);
            }
            var result = _memberService.UpdateMemberDetails(id, updatedMember);

            if (result)
            {
                TempData["SuccessMessage"] = "Member Updated Successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to Update Member.";
            }
            return RedirectToAction(nameof(Index));

        }

        public ActionResult Delete(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid member ID, Must be greater than zero.";
                return RedirectToAction(nameof(Index));
            }
            var member = _memberService.GetMemberDetails(id);
            if (member is null)
            {
                TempData["ErrorMessage"] = "Member not found.";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.MemberId = id;
            ViewBag.MemberName = member.Name;
            return View();
        }
        public ActionResult DeleteConfirmed([FromForm] int id) // using [FromForm] to explicitly bind the id from the form data , and use input type hidden in the view to pass the id value to this action method when the delete confirmation form is submitted.
        {
            var result = _memberService.DeleteMember(id);

            if (result)
                TempData["SuccessMessage"] = "Member Deleted Successfully.";
            else
                TempData["ErrorMessage"] = "Failed to Delete Member.";

            return RedirectToAction(nameof(Index));
        }
    }
}
