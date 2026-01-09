using GymMangmentBLL.Service.Interfaces;
using GymMangmentBLL.ViewModels.AccountViewModel;
using GymMangmerDAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GymMangmentPL.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(IAccountService accountService,SignInManager<ApplicationUser> signInManager) 
        {
            _accountService = accountService;
            _signInManager = signInManager;
        }
        //login
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var user = _accountService.ValidateUser(model);
            if (user is null)
            {
                ModelState.AddModelError("InvalidLogin", "Invalid Email or Password");
                return View(model);
            }
            var result = _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false).Result;
            if(result.IsNotAllowed)
                ModelState.AddModelError("NotAllowed", "You are not allowed to login");
            if(result.IsLockedOut)
                ModelState.AddModelError("LockedOut", "Your account is locked out");
            if (result.Succeeded)
                return RedirectToAction("Index", "Home");
            return View(model);
        }
        //logout
        public ActionResult Logout()
        {
            _signInManager.SignOutAsync().GetAwaiter().GetResult();
            return RedirectToAction(nameof(Login));
        }
        //accessdenied
        public ActionResult AccessDenied()
        {
            return View();
        }
    }
}
