using Xunit;
using NatureQuest.Services;
using NatureQuest.Models;
using NatureQuest.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace NatureQuest.Tests
{
    public class LocationServiceTests
    {
        private ApplicationDbContext GetDbContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            var context = new ApplicationDbContext(options);

            // Seed some data for testing if none exists
            if (!context.Locations.Any())
            {
                context.Locations.Add(new Location { Id = 1, LocationName = "TestLocation" });
                context.SaveChanges();
            }

            return context;
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllLocations()
        {
            var context = GetDbContext("GetAllLocDb");
            var service = new LocationService(context);

            var result = await service.GetAllAsync();
            var list = result.ToList();

            Assert.Single(list);
            Assert.Equal("TestLocation", list[0].LocationName);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsCorrectLocationOrNull()
        {
            var context = GetDbContext("GetByIdLocDb");
            var service = new LocationService(context);

            var loc = await service.GetByIdAsync(1);
            Assert.NotNull(loc);
            Assert.Equal("TestLocation", loc.LocationName);

            var missing = await service.GetByIdAsync(999);
            Assert.Null(missing);
        }

        [Fact]
        public async Task AddAsync_AddsLocation()
        {
            var context = GetDbContext("AddLocDb");
            var service = new LocationService(context);

            await service.AddAsync(new Location { LocationName = "NewLocation" });

            var all = await service.GetAllAsync();
            Assert.Equal(2, all.Count());
        }

        [Fact]
        public async Task UpdateAsync_UpdatesExistingLocation()
        {
            var context = GetDbContext("UpdateLocDb");
            var service = new LocationService(context);

            var loc = await service.GetByIdAsync(1);
            loc.LocationName = "UpdatedLocation";
            await service.UpdateAsync(loc);

            var updated = await service.GetByIdAsync(1);
            Assert.Equal("UpdatedLocation", updated.LocationName);
        }

        [Fact]
        public async Task DeleteAsync_DeletesExistingLocation()
        {
            var context = GetDbContext("DeleteLocDb");
            var service = new LocationService(context);

            await service.DeleteAsync(1);

            var loc = await service.GetByIdAsync(1);
            Assert.Null(loc);

            // Deleting non-existing should not throw
            await service.DeleteAsync(999);
        }
    }
}
