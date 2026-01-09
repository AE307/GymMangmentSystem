using GymMangmentBLL.Service.Interfaces;
using GymMangmentBLL.ViewModels.AccountViewModel;
using GymMangmerDAL.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangmentBLL.Service.Classes
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public ApplicationUser? ValidateUser(LoginViewModel loginViewModel)
        {
            var user = _userManager.FindByEmailAsync(loginViewModel.Email).Result;
            if (user is null) return null;
            var isPasswordValid = _userManager.CheckPasswordAsync(user, loginViewModel.Password).Result;
            return isPasswordValid ? user : null;
        }
    }
}
