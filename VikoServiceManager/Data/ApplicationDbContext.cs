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
        public DbSet<ResidentGroupViewModel> ResidentGroup { get; set; }
        public DbSet<ResidentGroupMembershipViewModel> ResidentGroupMembership { get; set; }
        public DbSet<HouseViewModel> House { get; set; }
        public DbSet<HouseServiceViewModel> HouseService { get; set; }
        public DbSet<ServiceViewModel> Service { get; set; }

    }
}
