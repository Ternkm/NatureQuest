using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NatureQuest.Models;
using NatureQuest.Services;
using NatureQuest.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NatureQuest.Controllers
{
    public class ObservationController : Controller
    {
        private readonly ObservationService _service;

        public ObservationController(ObservationService service)
        {
            _service = service;
        }

        // GET: Observation
        public async Task<IActionResult> Index()
        {
            var observations = await _service.GetAllObservationsAsync();

            var vm = observations.Select(o => new ObservationViewModel
            {
                ObservationId = o.Id,
                SpeciesName = o.SpeciesName,
                LocationName = o.LocationName,
                Latitude = o.Latitude,
                Longitude = o.Longitude,
                DateObserved = o.DateObserved,
                Notes = o.Notes,
                ImagePath = o.ImagePath
            }).ToList();

            return View(vm);
        }

        // GET: Observation/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var obs = await _service.GetObservationByIdAsync(id);
            if (obs == null) return NotFound();

            var vm = new ObservationViewModel
            {
                ObservationId = obs.Id,
                SpeciesName = obs.SpeciesName,
                LocationName = obs.LocationName,
                Latitude = obs.Latitude,
                Longitude = obs.Longitude,
                DateObserved = obs.DateObserved,
                Notes = obs.Notes,
                ImagePath = obs.ImagePath
            };

            return View(vm);
        }

        // GET: Observation/Create
        public async Task<IActionResult> Create()
        {
            var vm = new ObservationViewModel();
            await PopulateDropdownsWithDefaults(vm);
            return View(vm);
        }

        // POST: Observation/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ObservationViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropdownsWithDefaults(vm);
                return View(vm);
            }

            // Add species if not exists
            if (!string.IsNullOrWhiteSpace(vm.SpeciesName))
            {
                var existingSpecies = (await _service.GetAllSpeciesAsync())
                                        .FirstOrDefault(s => s.CommonName.ToLower() == vm.SpeciesName.ToLower());
                if (existingSpecies == null)
                    await _service.AddSpeciesAsync(new Species { CommonName = vm.SpeciesName });
            }

            // Add location if not exists
            if (!string.IsNullOrWhiteSpace(vm.LocationName))
            {
                var existingLocation = (await _service.GetAllLocationsAsync())
                                        .FirstOrDefault(l => l.LocationName.ToLower() == vm.LocationName.ToLower());
                if (existingLocation == null)
                    await _service.AddLocationAsync(new Location { LocationName = vm.LocationName });
            }

            var obs = new Observation
            {
                SpeciesName = vm.SpeciesName,
                LocationName = vm.LocationName,
                Latitude = vm.Latitude,
                Longitude = vm.Longitude,
                DateObserved = vm.DateObserved,
                Notes = vm.Notes,
                ImagePath = vm.ImagePath
            };

            await _service.AddObservationAsync(obs);
            return RedirectToAction(nameof(Index));
        }

        // GET: Observation/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var obs = await _service.GetObservationByIdAsync(id);
            if (obs == null) return NotFound();

            var vm = new ObservationViewModel
            {
                ObservationId = obs.Id,
                SpeciesName = obs.SpeciesName,
                LocationName = obs.LocationName,
                Latitude = obs.Latitude,
                Longitude = obs.Longitude,
                DateObserved = obs.DateObserved,
                Notes = obs.Notes,
                ImagePath = obs.ImagePath
            };

            await PopulateDropdownsWithDefaults(vm);
            return View(vm);
        }

        // POST: Observation/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ObservationViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropdownsWithDefaults(vm);
                return View(vm);
            }

            // Add species if not exists
            if (!string.IsNullOrWhiteSpace(vm.SpeciesName))
            {
                var existingSpecies = (await _service.GetAllSpeciesAsync())
                                        .FirstOrDefault(s => s.CommonName.ToLower() == vm.SpeciesName.ToLower());
                if (existingSpecies == null)
                    await _service.AddSpeciesAsync(new Species { CommonName = vm.SpeciesName });
            }

            // Add location if not exists
            if (!string.IsNullOrWhiteSpace(vm.LocationName))
            {
                var existingLocation = (await _service.GetAllLocationsAsync())
                                        .FirstOrDefault(l => l.LocationName.ToLower() == vm.LocationName.ToLower());
                if (existingLocation == null)
                    await _service.AddLocationAsync(new Location { LocationName = vm.LocationName });
            }

            var obs = new Observation
            {
                Id = vm.ObservationId,
                SpeciesName = vm.SpeciesName,
                LocationName = vm.LocationName,
                Latitude = vm.Latitude,
                Longitude = vm.Longitude,
                DateObserved = vm.DateObserved,
                Notes = vm.Notes,
                ImagePath = vm.ImagePath
            };

            await _service.UpdateObservationAsync(obs);
            return RedirectToAction(nameof(Index));
        }

        // GET: Observation/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var obs = await _service.GetObservationByIdAsync(id);
            if (obs == null) return NotFound();

            var vm = new ObservationViewModel
            {
                ObservationId = obs.Id,
                SpeciesName = obs.SpeciesName,
                LocationName = obs.LocationName,
                Latitude = obs.Latitude,
                Longitude = obs.Longitude,
                DateObserved = obs.DateObserved,
                Notes = obs.Notes,
                ImagePath = obs.ImagePath
            };

            return View(vm);
        }

        // POST: Observation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _service.DeleteObservationAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // --- Helper Methods ---

        private async Task PopulateDropdownsWithDefaults(ObservationViewModel vm)
        {
            var species = (await _service.GetAllSpeciesAsync()).ToList();
            var locations = (await _service.GetAllLocationsAsync()).ToList();

            // Default species
            if (!species.Any())
            {
                var defaultSpecies = new List<Species>
                {
                    new Species { CommonName = "Bald Eagle" },
                    new Species { CommonName = "Red Fox" },
                    new Species { CommonName = "Monarch Butterfly" },
                    new Species { CommonName = "American Robin"},
                    new Species { CommonName = "Raccoon" },
                    new Species { CommonName = "Eastern Gray Squirrel" },
                    new Species { CommonName = "Painted Turtle" },
                    new Species { CommonName = "American Goldfinch" },
                };
                foreach (var s in defaultSpecies)
                    await _service.AddSpeciesAsync(s);
                species = (await _service.GetAllSpeciesAsync()).ToList();
            }

            // Default locations
            if (!locations.Any())
            {
                var defaultLocations = new List<Location>
                {
                    new Location { LocationName = "Prairie Preserve" },
                    new Location { LocationName = "River Bend" },
                    new Location { LocationName = "Forest Glade" }
                };
                foreach (var l in defaultLocations)
                    await _service.AddLocationAsync(l);
                locations = (await _service.GetAllLocationsAsync()).ToList();
            }

            vm.SpeciesList = species.Select(s => new SelectListItem { Value = s.CommonName, Text = s.CommonName });
            vm.LocationList = locations.Select(l => new SelectListItem { Value = l.LocationName, Text = l.LocationName });
        }
    }
}
