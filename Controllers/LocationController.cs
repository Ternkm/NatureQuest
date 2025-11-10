using Microsoft.AspNetCore.Mvc;
using NatureQuest.Models;
using NatureQuest.Services;

namespace NatureQuest.Controllers
{
    public class LocationController : Controller
    {
        private readonly ILocationService _service;

        public LocationController(ILocationService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
            => View(await _service.GetAllAsync());

        public async Task<IActionResult> Details(int id)
        {
            var location = await _service.GetByIdAsync(id);
            if (location == null) return NotFound();
            return View(location);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(Location location)
        {
            if (ModelState.IsValid)
            {
                await _service.AddAsync(location);
                return RedirectToAction(nameof(Index));
            }
            return View(location);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var location = await _service.GetByIdAsync(id);
            if (location == null) return NotFound();
            return View(location);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Location location)
        {
            if (ModelState.IsValid)
            {
                await _service.UpdateAsync(location);
                return RedirectToAction(nameof(Index));
            }
            return View(location);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var location = await _service.GetByIdAsync(id);
            if (location == null) return NotFound();
            return View(location);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }

}
