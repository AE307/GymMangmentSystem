using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangmentBLL.ViewModels.AccountViewModel
{
    public class LoginViewModel
    {
        [Required (ErrorMessage="Email is Requried")]
        public string Email { get; set; } = null!;
        [Required(ErrorMessage = "Password is Requried")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
        public bool RememberMe { get; set; }

    }
}
