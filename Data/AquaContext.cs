
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Data
{
    public class AquaContext : DbContext
    {
        public DbSet<WaterSource> WaterSources { get; set; }
        public DbSet<WaterSourceUpdate> WaterSourceUpdates { get; set; }
        public AquaContext(DbContextOptions<AquaContext> options) : base(options)
        {
        }
    }
}
