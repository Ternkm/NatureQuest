using Microsoft.AspNetCore.Mvc;
using NatureQuest.Models;
using NatureQuest.Services;

namespace NatureQuest.Controllers
{
    public class SpeciesController : Controller
    {
        private readonly ISpeciesService _speciesService;

        // Inject SpeciesService
        public SpeciesController(ISpeciesService speciesService)
        {
            _speciesService = speciesService;
        }

        // GET: Species
        public async Task<IActionResult> Index()
        {
            var speciesList = _speciesService.GetAllAsync();
            return View(speciesList);
        }

        // GET: Species/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var species = _speciesService.GetByIdAsync(id);
            if (species == null)
                return NotFound();
            return View(species);
        }

        // GET: Species/Create
        public IActionResult Create() => View();

        // POST: Species/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Species species)
        {
            if (ModelState.IsValid)
            {
                await _speciesService.AddAsync(species);
                return RedirectToAction(nameof(Index));
            }
            return View(species);
        }

        // GET: Species/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var species = await _speciesService.GetByIdAsync(id);
            if (species == null)
                return NotFound();
            return View(species);
        }

        // POST: Species/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Species species)
        {
            if (ModelState.IsValid)
            {
                await _speciesService.UpdateAsync(species);
                return RedirectToAction(nameof(Index));
            }
            return View(species);
        }

        // GET: Species/Delete/5
        public IActionResult Delete(int id)
        {
            var species = _speciesService.GetByIdAsync(id);
            if (species == null)
                return NotFound();
            return View(species);
        }

        // POST: Species/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _speciesService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
