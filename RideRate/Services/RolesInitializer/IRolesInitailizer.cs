using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RideRate.Models;

namespace RideRate.Services.RolesInitializer
{
    public interface IRolesInitailizer
    {
        void Initialize(UserManager<ApplicationUser> userManager);
    }
}
