using NatureQuest.Data;
using NatureQuest.Models;
using Microsoft.EntityFrameworkCore;

namespace NatureQuest.Services
{
    public class LocationService : ILocationService
    {
        private readonly ApplicationDbContext _context;

        public LocationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Location>> GetAllAsync()
            => await _context.Locations.ToListAsync();

        public async Task<Location?> GetByIdAsync(int id)
            => await _context.Locations.FindAsync(id);

        public async Task AddAsync(Location location)
        {
            _context.Locations.Add(location);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Location location)
        {
            _context.Locations.Update(location);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var loc = await _context.Locations.FindAsync(id);
            if (loc != null)
            {
                _context.Locations.Remove(loc);
                await _context.SaveChangesAsync();
            }
        }
    }

}
