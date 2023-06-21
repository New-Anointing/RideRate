using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RideRate.Data;
using RideRate.Models;
using RideRate.Utilities;

namespace RideRate.Services.RolesInitializer
{
    internal class Users : ApplicationUser
    {
    }
    public class RolesInitailizer : IRolesInitailizer
    {
        private readonly ApiDbcontext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        public RolesInitailizer(ApiDbcontext context, RoleManager<IdentityRole> roleManager)
        {
            _context=context;
            _roleManager=roleManager;
        }

        public void Initialize(UserManager<ApplicationUser> userManager)
        {
            try
            {
                if (_context.Database.GetPendingMigrations().Count()>0)
                {
                    _context.Database.Migrate();
                }
            }catch(Exception ex)
            {

            }
            if (_context.Roles.Any(r => r.Name == SD.Consumers) && _context.Roles.Any(r => r.Name == SD.Contributors) && _context.Roles.Any(r => r.Name == SD.SuperUser)) return;
            
            //ROLES CREATION
            _roleManager.CreateAsync(new IdentityRole(SD.Contributors)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(SD.Consumers)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(SD.SuperUser)).GetAwaiter().GetResult();
            //Initialize the superuser if no user exist
            if (!userManager.Users.Any())
            {
                var user = new Users
                {
                    AppUserId = Guid.NewGuid(),
                    UserName = "aremunewanointing@gmail.com",
                    Email = "aremunewanointing@gmail.com",
                    FirstName = "New-Anointing",
                    LastName = "Aremu",
                    HomeAddress = "Default Address",
                    PhoneNumber = "08106917791",
                    Gender = "Default",
                    Role = SD.SuperUser,
                };
                try
                {
                    var result = userManager.CreateAsync(user, "Admin@123").GetAwaiter().GetResult();
                    if (result.Succeeded)
                    {
                        userManager.AddToRoleAsync(user, SD.SuperUser).GetAwaiter().GetResult();
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }
    }
}
