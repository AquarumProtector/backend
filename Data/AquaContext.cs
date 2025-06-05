
using backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace backend.Data
{
    public class AquaContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
    {
        public DbSet<WaterSource> WaterSources { get; set; }
        public DbSet<WaterSourceUpdate> WaterSourceUpdates { get; set; }
        public DbSet<Alert> Alerts { get; set; }
        public AquaContext(DbContextOptions<AquaContext> options) : base(options)
        {
        }
    }
}
