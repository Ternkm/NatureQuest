using NatureQuest.Data;
using NatureQuest.Models;
using NatureQuest.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System.Threading.Tasks;
using System.Linq;

public class ObservationServiceTests
{
    private ApplicationDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("ObservationTestDB")
            .Options;

        return new ApplicationDbContext(options);
    }

    [Fact]
    public async Task CanAddObservation()
    {
        var context = GetDbContext();
        var service = new ObservationService(context);

        var obs = new Observation
        {
            SpeciesName = "TestSpecies",
            LocationName = "TestLocation",
            Latitude = 1,
            Longitude = 2,
            DateObserved = System.DateTime.Now
        };

        await service.AddObservationAsync(obs);

        var all = await service.GetAllObservationsAsync();
        Assert.Contains(all, o => o.SpeciesName == "TestSpecies");
    }

    [Fact]
    public async Task CanGetObservationById()
    {
        var context = GetDbContext();
        var service = new ObservationService(context);

        var obs = new Observation
        {
            SpeciesName = "ByIdSpecies",
            LocationName = "ByIdLocation",
            Latitude = 1,
            Longitude = 2,
            DateObserved = System.DateTime.Now
        };

        await service.AddObservationAsync(obs);

        var retrieved = await service.GetObservationByIdAsync(obs.Id);
        Assert.NotNull(retrieved);
        Assert.Equal("ByIdSpecies", retrieved.SpeciesName);
    }

    [Fact]
    public async Task CanUpdateObservation()
    {
        var context = GetDbContext();
        var service = new ObservationService(context);

        var obs = new Observation
        {
            SpeciesName = "OldSpecies",
            LocationName = "OldLocation",
            Latitude = 1,
            Longitude = 2,
            DateObserved = System.DateTime.Now
        };

        await service.AddObservationAsync(obs);

        obs.SpeciesName = "NewSpecies";
        await service.UpdateObservationAsync(obs);

        var updated = await service.GetObservationByIdAsync(obs.Id);
        Assert.Equal("NewSpecies", updated.SpeciesName);
    }

    [Fact]
    public async Task CanDeleteObservation()
    {
        var context = GetDbContext();
        var service = new ObservationService(context);

        var obs = new Observation
        {
            SpeciesName = "DeleteSpecies",
            LocationName = "DeleteLocation",
            Latitude = 1,
            Longitude = 2,
            DateObserved = System.DateTime.Now
        };

        await service.AddObservationAsync(obs);
        await service.DeleteObservationAsync(obs.Id);

        var deleted = await service.GetObservationByIdAsync(obs.Id);
        Assert.Null(deleted);
    }
}

