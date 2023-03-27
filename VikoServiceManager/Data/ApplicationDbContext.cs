using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VikoServiceManager.Models;

namespace IdentityManager.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions options): base(options)
        {
            
        }

        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<ResidentGroup> ResidentGroup { get; set; }
        public DbSet<ResidentGroupMembership> ResidentGroupMembership { get; set; }
        public DbSet<House> House { get; set; }
        public DbSet<HouseService> HouseService { get; set; }
        public DbSet<Service> Service { get; set; }

    }
}
