using Microsoft.EntityFrameworkCore;
using NatureQuest.Models;

namespace NatureQuest.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Observation> Observations { get; set; }
        public DbSet<Species> Species { get; set; }
        public DbSet<Location> Locations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed data removed for now
        }
    }
}
