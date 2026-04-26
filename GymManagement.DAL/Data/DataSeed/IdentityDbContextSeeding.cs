using GymManagement.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Data.DataSeed
{
    public class IdentityDbContextSeeding
    {
        public static bool SeedData(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            try
            {
                var hasUsers = userManager.Users.Any();
                var hasRoles = roleManager.Roles.Any();

                if (hasUsers && hasRoles)
                {
                    return false;
                }

                if (!hasRoles)
                {
                    var roles = new List<IdentityRole>
                    {
                        new IdentityRole { Name = "SuperAdmin" },
                        new IdentityRole { Name = "Admin" }
                    };

                    foreach (var role in roles)
                    {
                        if (!roleManager.RoleExistsAsync(role.Name).Result)
                        {
                            roleManager.CreateAsync(role).Wait();
                        }
                    }
                }

                if (!hasUsers)
                {
                    var MainAdmin = new ApplicationUser
                    {
                        FirstName = "Omar",
                        LastName = "Gibreel",
                        UserName = "OmarGibreel",
                        Email = "omargibreell@gmail.com",
                        PhoneNumber = "01014413327"
                    };
                    userManager.CreateAsync(MainAdmin, "P@ssw0rd").Wait();
                    userManager.AddToRoleAsync(MainAdmin, "SuperAdmin").Wait();

                    var Admin = new ApplicationUser
                    {
                        FirstName = "Motaz",
                        LastName = "Essam",
                        UserName = "MotazEssam",
                        Email = "motazessam@gmail.com",
                        PhoneNumber = "01000000000"
                    };
                    userManager.CreateAsync(Admin, "P@ssw0rd").Wait();
                    userManager.AddToRoleAsync(Admin, "Admin").Wait();
                }

                return true;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Seeding Failed: {ex}");
                return false;
            }
        }
    }
}
