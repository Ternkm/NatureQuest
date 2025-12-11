using NatureQuest.Models;

namespace NatureQuest.Services
{
    public interface ISpeciesService
    {
        Task<IEnumerable<Species>> GetAllAsync();
        Task<Species?> GetByIdAsync(int id);
        Task AddAsync(Species species);
        Task UpdateAsync(Species species);
        Task DeleteAsync(int id);
    }

}
