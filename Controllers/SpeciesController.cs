using Microsoft.AspNetCore.Mvc;
using NatureQuest.Models;
using NatureQuest.Services;

namespace NatureQuest.Controllers
{
    public class SpeciesController: Controller
    {
        private readonly SpeciesService _speciesService;

        // Inject SpeciesService
        public SpeciesController(SpeciesService speciesService)
        {
            _speciesService = speciesService;
        }

        // GET: Species
        public IActionResult Index()
        {
            var speciesList = _speciesService.GetAllSpecies();
            return View(speciesList);
        }

        // GET: Species/Details/5
        public IActionResult Details(int id)
        {
            var species = _speciesService.GetSpeciesById(id);
            if (species == null)
                return NotFound();
            return View(species);
        }

        // GET: Species/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Species/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Species species)
        {
            if (ModelState.IsValid)
            {
                _speciesService.AddSpecies(species);
                return RedirectToAction(nameof(Index));
            }
            return View(species);
        }

        // GET: Species/Edit/5
        public IActionResult Edit(int id)
        {
            var species = _speciesService.GetSpeciesById(id);
            if (species == null)
                return NotFound();
            return View(species);
        }

        // POST: Species/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Species species)
        {
            if (ModelState.IsValid)
            {
                _speciesService.UpdateSpecies(species);
                return RedirectToAction(nameof(Index));
            }
            return View(species);
        }

        // GET: Species/Delete/5
        public IActionResult Delete(int id)
        {
            var species = _speciesService.GetSpeciesById(id);
            if (species == null)
                return NotFound();
            return View(species);
        }

        // POST: Species/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _speciesService.DeleteSpecies(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
