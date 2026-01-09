using GymMangmentBLL.Service.Interfaces;
using GymMangmentBLL.ViewModels.MemberViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymMangmentPL.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class MemberController : Controller
    {
        private readonly IMemberService _memberService;

        public MemberController(IMemberService memberService)
        {
            _memberService = memberService;
        }
        public ActionResult Index()
        {
            var members = _memberService.GetAllMember();
            return View(members);
        }
        public ActionResult MemberDetails(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "id of member can't be 0 or negative";
                return RedirectToAction(nameof(Index));
            }
            var member = _memberService.GetMemberDetails(id);
            if (member == null)
            {
                TempData["ErrorMessage"] = "member not found";
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }
        public ActionResult HealthRecordDetails(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "id of member can't be 0 or negative";
                return RedirectToAction(nameof(Index));
            }
            var healthRecord = _memberService.GetMemberHealthRecordDetails(id);
            if (healthRecord == null)
            {
                TempData["ErrorMessage"] = "member not found";
                return RedirectToAction(nameof(Index));
            }
            return View(healthRecord);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateMember(CreateMemberViewModel createMember)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("DataMissed", "check data and missing field");
                return View(nameof(Create), createMember);
            }
            bool result = _memberService.createMember(createMember);
            if (result)
            {
                TempData["SuccessMessage"] = "Member Created Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to Create Member";
            }
            return RedirectToAction(nameof(Index));
        }
        public ActionResult MemberEdit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "id of member can't be 0 or negative";
                return RedirectToAction(nameof(Index));
            }
            var Member = _memberService.GetMemberForUpdate(id);
            if (Member is null)
            {
                TempData["ErrorMessage"] = "member not found";
                return RedirectToAction(nameof(Index));
            }
            return View(Member);
        }
        [HttpPost]
        public ActionResult MemberEdit([FromRoute] int id, MemberToUpdateViewModel memberToUpdate)
        {
            if (!ModelState.IsValid)
            {
                return View(memberToUpdate);
            }
            var result = _memberService.UpdateMember(id, memberToUpdate);
            if (result)
            {
                TempData["SuccessMessage"] = "Member Updated Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to Update Member";
            }
            return RedirectToAction(nameof(Index));
        }
        public ActionResult Delete(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "id of member can't be 0 or negative";
                return RedirectToAction(nameof(Index));
            }
            var member = _memberService.GetMemberDetails(id);
            if (member is null)
            {
                TempData["ErrorMessage"] = "member not found";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.MemberId = id;
            return View();
        }
        [HttpPost]
        public ActionResult DeleteConfirm([FromForm]int id)
        {
            var result = _memberService.RemoveMember(id);
            if (result)
            {
                TempData["SuccessMessage"] = "Member delete Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to delete Member";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
