using GymMangmentBLL.Service.Interfaces;
using GymMangmentBLL.ViewModels.SessionViewModel;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymMangmentPL.Controllers
{
    public class SessionController : Controller
    {
        private readonly ISessionService _sessionService;

        public SessionController(ISessionService sessionService) 
        {
            _sessionService = sessionService;
        }
        public ActionResult Index()
        {
            var sessions = _sessionService.GetAllSessions();
            return View(sessions);
        }
        public ActionResult Details(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid session ID.";
                return RedirectToAction(nameof(Index));
            }
            var session = _sessionService.GetSessionById(id);
            if (session is null)
            {
                TempData["ErrorMessage"] = "Session not found.";
                return RedirectToAction(nameof(Index));
            }
            return View(session);

        }
        public ActionResult Create() 
        {
            LoadDropDownTrainer();
            LoadDropDownCategory();
            return View();
        }
        [HttpPost]
        public ActionResult Create(CreateSessionViewModel createSession)
        {
            if (!ModelState.IsValid)
            {
                LoadDropDownTrainer();
                LoadDropDownCategory();
                return View(createSession);
            }
            var result = _sessionService.CreateSession(createSession);
            if (result)
            {
                TempData["SuccessMessage"] = "Session created successfully.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to create session.";
                LoadDropDownTrainer();
                LoadDropDownCategory();
                return View(createSession);
            }
        }
        public ActionResult Edit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid session ID.";
                return RedirectToAction(nameof(Index));
            }
            var session = _sessionService.GetSessionToUpdate(id);
            if (session is null)
            {
                TempData["ErrorMessage"] = "Session not found.";
                return RedirectToAction(nameof(Index));
            }
            LoadDropDownTrainer();
            return View(session);
        }
        [HttpPost]
        public ActionResult Edit([FromRoute]int id,UpdateSessionViewModel updateSession)
        {
            if (!ModelState.IsValid)
            {
                LoadDropDownTrainer();
                return View(updateSession);
            }
            var result = _sessionService.UpdateSession(id, updateSession);
            if (result)
            {
                TempData["SuccessMessage"] = "Session updated successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to update session.";
            }
            return RedirectToAction(nameof(Index));
        }
        public ActionResult Delete(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid session ID.";
                return RedirectToAction(nameof(Index));
            }
            var session = _sessionService.GetSessionById(id);
            if (session is null)
            {
                TempData["ErrorMessage"] = "Failed to delete session.";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.SessionId = session.Id;
            return View(session);
        }
        [HttpPost]
        public ActionResult DeleteConfirmed(int id)
        {
            var result = _sessionService.RemoveSession(id);
            if (result)
            {
                TempData["SuccessMessage"] = "Session deleted successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to delete session.";
            }
            return RedirectToAction(nameof(Index));
        }
        private void LoadDropDownCategory()
        {
            var category = _sessionService.GetCategoryForDropDown();
            ViewBag.Categories = new SelectList(category, "Id", "Name");
        }
        private void LoadDropDownTrainer()
        {
            var trainer = _sessionService.GetTrainerForDropDown();
            ViewBag.Trainers = new SelectList(trainer, "Id", "Name");
        }
    }
}
