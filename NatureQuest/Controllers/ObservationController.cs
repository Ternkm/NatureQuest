using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using NatureQuest.Models;
using NatureQuest.Services;
using NatureQuest.ViewModels;
using NatureQuest.Filters;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing.Text;
using NatureQuest.Controllers;

namespace NatureQuest.Controllers
{
    [ServiceFilter(typeof(DropdownPopulateFilter))] // Auto-populate dropdowns
    public class ObservationController : Controller
    {
        private readonly ObservationService _service;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<ObservationController> _logger;

        public ObservationController(ObservationService service, IWebHostEnvironment webHostEnvironment, ILogger<ObservationController> logger)
        {
            _service = service;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }

        // GET: Observation
        public async Task<IActionResult> Index()
        {
            var observations = await _service.GetAllObservationsAsync();
            var vmList = observations.Select(o => new ObservationViewModel
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

            return View(vmList);
        }

        // GET: Observation/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var obs = await _service.GetObservationByIdAsync(id);
            if (obs == null) return NotFound();
            return View(MapObservationToViewModel(obs));
        }

        // GET: Observation/Create
        [Authorize(Roles = "Guest, Admin")]
        public IActionResult Create()
        {
            return View(new ObservationViewModel());
        }

        // POST: Observation/Create
        [HttpPost]
        [Authorize(Roles = "Guest, Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ObservationViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            await AddSpeciesAndLocationIfMissing(vm);

            
            string savedImagePath = null;
            if (vm.ImageFile != null)
            {
                savedImagePath = await SaveImageFile(vm.ImageFile);
            }

            var obs = new Observation
            {
                SpeciesName = vm.SpeciesName,
                LocationName = vm.LocationName,
                Latitude = vm.Latitude,
                Longitude = vm.Longitude,
                DateObserved = vm.DateObserved,
                Notes = vm.Notes,
                ImagePath = savedImagePath    // store uploaded file path
            };

            await _service.AddObservationAsync(obs);
            return RedirectToAction(nameof(Index));
        }

        // GET: Observation/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var obs = await _service.GetObservationByIdAsync(id.Value);
            if (obs == null) return NotFound();

            return View(MapObservationToViewModel(obs));
        }

        // POST: Observation/Edit/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ObservationViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            await AddSpeciesAndLocationIfMissing(vm);

            string imagePath = vm.ImagePath;

            
            if (vm.ImageFile != null)
            {
                imagePath = await SaveImageFile(vm.ImageFile);
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
                ImagePath = imagePath
            };

            await _service.UpdateObservationAsync(obs);
            return RedirectToAction(nameof(Index));
        }

        // GET: Observation/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var obs = await _service.GetObservationByIdAsync(id);
            if (obs == null) return NotFound();
            return View(MapObservationToViewModel(obs));
        }

        // POST: Observation/Delete/5
        [HttpPost, ActionName("DeleteConfirmed")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int ObservationId)
        {
            await _service.DeleteObservationAsync(ObservationId);
            return RedirectToAction(nameof(Index));
        }

        // --- Helper Methods ---
        private async Task AddSpeciesAndLocationIfMissing(ObservationViewModel vm)
        {
            if (!string.IsNullOrWhiteSpace(vm.SpeciesName))
            {
                var existingSpecies = (await _service.GetAllSpeciesAsync())
                    .FirstOrDefault(s => s.CommonName.Equals(vm.SpeciesName, StringComparison.OrdinalIgnoreCase));
                if (existingSpecies == null)
                    await _service.AddSpeciesAsync(new Species { CommonName = vm.SpeciesName });
            }

            if (!string.IsNullOrWhiteSpace(vm.LocationName))
            {
                var existingLocation = (await _service.GetAllLocationsAsync())
                    .FirstOrDefault(l => l.LocationName.Equals(vm.LocationName, StringComparison.OrdinalIgnoreCase));
                if (existingLocation == null)
                    await _service.AddLocationAsync(new Location { LocationName = vm.LocationName });
            }
        }

        private ObservationViewModel MapObservationToViewModel(Observation o) => new ObservationViewModel
        {
            ObservationId = o.Id,
            SpeciesName = o.SpeciesName,
            LocationName = o.LocationName,
            Latitude = o.Latitude,
            Longitude = o.Longitude,
            DateObserved = o.DateObserved,
            Notes = o.Notes,
            ImagePath = o.ImagePath
        };

        private async Task<string> SaveImageFile(Microsoft.AspNetCore.Http.IFormFile file)
        {
            if (file == null || file.Length == 0)
                return null;

            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            try
            {
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Upload failed while saving {FilePath}", filePath);
                throw;
            }

            return $"/images/{uniqueFileName}";
        }
    }
}
