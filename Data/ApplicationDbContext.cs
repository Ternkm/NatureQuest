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

            // Seed Locations
            modelBuilder.Entity<Location>().HasData(
                new Location { Id = 1, LocationName = "Prairie Preserve", Latitude = 41.6, Longitude = -93.6 },
                new Location { Id = 2, LocationName = "River Bend", Latitude = 41.7, Longitude = -93.7 }
            );

            // Seed Observations (linking to Locations and Species)
            //modelBuilder.Entity<Observation>().HasData(
            //    new Observation { Id = 1, SpeciesId = 1, LocationId = 1, DateObserved = DateTime.Now },
            //    new Observation { Id = 2, SpeciesId = 2, LocationId = 2, DateObserved = DateTime.Now }
            //);
        }
    }
    }
