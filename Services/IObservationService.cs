using NatureQuest.Models;

public interface IObservationService
{
    Task<IEnumerable<Observation>> GetAllObservationsAsync();
    Task<Observation?> GetObservationByIdAsync(int id);
    Task AddObservationAsync(Observation observation);
    Task UpdateObservationAsync(Observation observation);
    Task DeleteObservationAsync(int id);
    Task<IEnumerable<Species>> GetAllSpeciesAsync();
    Task<IEnumerable<Location>> GetAllLocationsAsync();
}
