using GymMangmerDAL.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangmerDAL.Data.DataSeeding
{
    public static class IdentityDbContextSeeding
    {
        public static bool SeedData(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            try
            {
                var HasUser = userManager.Users.Any();
                var HasRole = roleManager.Roles.Any();
                if (HasUser && HasRole) return false;
                if (!HasRole)
                {
                    var Roles = new List<IdentityRole>()
                    {
                        new () { Name= "SuperAdmin"},
                        new () { Name= "Admin" }
                    };
                    foreach (var role in Roles)
                    {
                        if (!roleManager.RoleExistsAsync(role.Name!).Result)
                        {
                            roleManager.CreateAsync(role).Wait();
                        }

                    }
                }
                if (!HasUser)
                {
                    var MainAdmin = new ApplicationUser()
                    {
                        FirstName = "Ahmed",
                        LastName = "Elsayed",
                        UserName = "AhmedElsayed",
                        Email="Ahmed@gmail.com",
                        PhoneNumber = "01234567890"
                    };
                    userManager.CreateAsync(MainAdmin,"P@ssw0rd").Wait();
                    userManager.AddToRoleAsync(MainAdmin, "SuperAdmin").Wait();

                    var Admin = new ApplicationUser()
                    {
                        FirstName = "Mohamed",
                        LastName = "Amr",
                        UserName = "MohamedAmr",
                        Email = "Mohamed@gmail.com",
                        PhoneNumber = "01234567891"
                    };
                    userManager.CreateAsync(Admin, "P@ssw0rd").Wait();
                    userManager.AddToRoleAsync(Admin, "Admin").Wait();
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Seeding Failed : {ex}");
                return false;
            }
        }
    }
}
