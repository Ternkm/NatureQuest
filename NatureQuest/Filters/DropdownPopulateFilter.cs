using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using NatureQuest.Services;
using NatureQuest.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NatureQuest.Filters
{
    public class DropdownPopulateFilter : IAsyncActionFilter
    {
        private readonly ObservationService _service;

        public DropdownPopulateFilter(ObservationService service)
        {
            _service = service;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next(); // Execute the action first

            if (resultContext.Result is ViewResult viewResult && viewResult.Model is ObservationViewModel vm)
            {
                var species = (await _service.GetAllSpeciesAsync()).ToList();
                var locations = (await _service.GetAllLocationsAsync()).ToList();

                // Populate default species if database empty
                if (!species.Any())
                {
                    var defaultSpecies = new List<Models.Species>
                    {
                        new Models.Species { CommonName = "Bald Eagle" },
                        new Models.Species { CommonName = "Red Fox" },
                        new Models.Species { CommonName = "Monarch Butterfly" },
                        new Models.Species { CommonName = "American Robin"},
                        new Models.Species { CommonName = "Raccoon" },
                        new Models.Species { CommonName = "Eastern Gray Squirrel" },
                        new Models.Species { CommonName = "Painted Turtle" },
                        new Models.Species { CommonName = "American Goldfinch" },
                    };
                    foreach (var s in defaultSpecies)
                        await _service.AddSpeciesAsync(s);
                    species = (await _service.GetAllSpeciesAsync()).ToList();
                }

                // Populate default locations if database empty
                if (!locations.Any())
                {
                    var defaultLocations = new List<Models.Location>
                    {
                        new Models.Location { LocationName = "Prairie Preserve" },
                        new Models.Location { LocationName = "River Bend" },
                        new Models.Location { LocationName = "Forest Glade" }
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
}

