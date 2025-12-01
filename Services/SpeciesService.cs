using System.Collections.Generic;
using System.Threading.Tasks;
using NatureQuest.Models;

namespace NatureQuest.Services
{
    public interface SpeciesService
    {
        Task<IEnumerable<Species>> GetAllAsync();
        Task<Species?> GetByIdAsync(int id);
    }
}
