//using Xunit;
//using NatureQuest.Services;
//using NatureQuest.Models;
//using System.Linq;

//public class SpeciesServiceTests
//{
//    [Fact]
//    public void AddSpecies_ShouldAddSpecies()
//    {
//        var service = new SpeciesService();
//        var species = new Species { Name = "Lion" };

//        service.AddSpecies(species);

//        Assert.Contains(species, service.GetAllSpecies().ToList());
//    }

//    [Fact]
//    public void DeleteSpecies_ShouldRemoveSpecies()
//    {
//        var service = new SpeciesService();
//        var species = new Species { Id = 1, Name = "Tiger" };
//        service.AddSpecies(species);

//        service.DeleteSpecies(1);

//        Assert.DoesNotContain(species, service.GetAllSpecies().ToList());
//    }
//}
