using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NatureQuest.Models;
using NatureQuest.ViewModels;
using NatureQuest.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NatureQuest.Filters
{
    public class ObservationMappingFilter : IAsyncActionFilter
    {
        private readonly ObservationService _service;

        public ObservationMappingFilter(ObservationService service)
        {
            _service = service;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next(); // Let action execute first

            if (resultContext.Result is ViewResult viewResult)
            {
                if (viewResult.Model is Observation observation)
                {
                    viewResult.ViewData["ObservationViewModel"] = MapObservationToViewModel(observation);
                }
                else if (viewResult.Model is IEnumerable<Observation> observations)
                {
                    viewResult.ViewData["ObservationViewModelList"] =
                        observations.Select(MapObservationToViewModel).ToList();
                }
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
    }
}
