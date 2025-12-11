using NatureQuest.Data;
using NatureQuest.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NatureQuest.Services
{
    public class ObservationService
    {
        private readonly ApplicationDbContext _context;

        public ObservationService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Observations CRUD
        public async Task<IEnumerable<Observation>> GetAllObservationsAsync()
        {
            return await _context.Observations.ToListAsync();
        }

        public async Task<Observation?> GetObservationByIdAsync(int id)
        {
            return await _context.Observations.FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task AddObservationAsync(Observation observation)
        {
            await _context.Observations.AddAsync(observation);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateObservationAsync(Observation observation)
        {
            _context.Observations.Update(observation);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteObservationAsync(int id)
        {
            var obs = await _context.Observations.FindAsync(id);
            if (obs != null)
            {
                _context.Observations.Remove(obs);
                await _context.SaveChangesAsync();
            }
        }

        // Dropdown helpers
        public async Task<IEnumerable<Species>> GetAllSpeciesAsync()
        {
            return await _context.Species.ToListAsync();
        }

        public async Task<IEnumerable<Location>> GetAllLocationsAsync()
        {
            return await _context.Locations.ToListAsync();
        }

        // NEW: Add species if not exists
        public async Task AddSpeciesAsync(Species species)
        {
            await _context.Species.AddAsync(species);
            await _context.SaveChangesAsync();
        }

        // NEW: Add location if not exists
        public async Task AddLocationAsync(Location location)
        {
            await _context.Locations.AddAsync(location);
            await _context.SaveChangesAsync();
        }
    }
}
