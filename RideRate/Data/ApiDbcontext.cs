using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RideRate.Models;

namespace RideRate.Data
{
    public class ApiDbcontext : IdentityDbContext
    {
        public ApiDbcontext(DbContextOptions<ApiDbcontext> options)
            : base(options)
        {

        }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<Location> Location { get; set; }
        public DbSet<Rate> Rate { get; set; }
    }
}
