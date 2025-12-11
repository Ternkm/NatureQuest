using Xunit;
using NatureQuest.Services;
using NatureQuest.Models;
using NatureQuest.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace NatureQuest.Tests
{
    public class SpeciesServiceTests
    {
        private ApplicationDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("SpeciesTestDB")
                .Options;

            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task CanAddAndGetSpecies()
        {
            var context = GetDbContext();
            var service = new SpeciesService(context);

            var sp = new Species { CommonName = "TestSpecies" };
            await service.AddAsync(sp);

            var all = await service.GetAllAsync();
            Assert.Contains(all, s => s.CommonName == "TestSpecies");
        }

        [Fact]
        public async Task CanGetSpeciesById()
        {
            var context = GetDbContext();
            var service = new SpeciesService(context);

            var sp = new Species { CommonName = "ByIdSpecies" };
            await service.AddAsync(sp);

            var fetched = await service.GetByIdAsync(sp.Id);
            Assert.NotNull(fetched);
            Assert.Equal("ByIdSpecies", fetched.CommonName);
        }

        [Fact]
        public async Task CanUpdateSpecies()
        {
            var context = GetDbContext();
            var service = new SpeciesService(context);

            var sp = new Species { CommonName = "OldName" };
            await service.AddAsync(sp);

            sp.CommonName = "NewName";
            await service.UpdateAsync(sp);

            var updated = await service.GetByIdAsync(sp.Id);
            Assert.Equal("NewName", updated.CommonName);
        }

        [Fact]
        public async Task CanDeleteSpecies()
        {
            var context = GetDbContext();
            var service = new SpeciesService(context);

            var sp = new Species { CommonName = "ToDelete" };
            await service.AddAsync(sp);

            await service.DeleteAsync(sp.Id);

            var deleted = await service.GetByIdAsync(sp.Id);
            Assert.Null(deleted);
        }
    }
}
