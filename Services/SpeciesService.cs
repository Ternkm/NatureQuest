using NatureQuest.Data;
using NatureQuest.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace NatureQuest.Services
{
    public class SpeciesService : ISpeciesService
    {
        private readonly ApplicationDbContext _context;

        public SpeciesService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Species>> GetAllAsync()
            => await _context.Species.ToListAsync();

        public async Task<Species?> GetByIdAsync(int id)
            => await _context.Species.FindAsync(id);

        public async Task AddAsync(Species species)
        {
            _context.Species.Add(species);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Species species)
        {
            _context.Species.Update(species);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var sp = await _context.Species.FindAsync(id);
            if (sp != null)
            {
                _context.Species.Remove(sp);
                await _context.SaveChangesAsync();
            }
        }
    }
}
