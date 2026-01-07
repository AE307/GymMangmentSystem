using GymMangmentBLL.Service.Interfaces;
using GymMangmentBLL.ViewModels.PlanViewModel;
using Microsoft.AspNetCore.Mvc;

namespace GymMangmentPL.Controllers
{
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
        public ActionResult Details(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid plan ID.";
                return RedirectToAction(nameof(Index));
            }
            var plan = _planService.GetPlaneById(id);
            if (plan is null)
            {
                TempData["ErrorMessage"] = "Plan not found.";
                return RedirectToAction(nameof(Index));
            }
            return View(plan);
        }
        public ActionResult Edit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid plan ID.";
                return RedirectToAction(nameof(Index));
            }
            var plan = _planService.GetPlaneToUpdate(id);
            if (plan is null)
            {
                TempData["ErrorMessage"] = "Plan can not be updated";
                return RedirectToAction(nameof(Index));
            }
            return View(plan);
        }
        [HttpPost]
        public ActionResult Edit([FromRoute]int id,UpdatePlanViewModel updatePlan)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Wrong Data", "Check data validation");
                return View(updatePlan);
            }
            var result = _planService.UpdatePlan(id, updatePlan);
            if (result)
            {
                TempData["SuccessMessage"] = "Plan updated Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Plan can not be updated";
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public ActionResult Activate([FromRoute]int id) 
        {
            var result = _planService.ToggleStatus(id);
            if (result)
            {
                TempData["SuccessMessage"] = "Plan status Changed";
            }
            else
            {
                TempData["ErrorMessage"] = "Plan status can not change";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
