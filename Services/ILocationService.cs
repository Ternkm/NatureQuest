using NatureQuest.Models;

namespace NatureQuest.Services
{
    public interface ILocationService
    {
        Task<IEnumerable<Location>> GetAllAsync();
        Task<Location?> GetByIdAsync(int id);
        Task AddAsync(Location location);
        Task UpdateAsync(Location location);
        Task DeleteAsync(int id);
    }
}
