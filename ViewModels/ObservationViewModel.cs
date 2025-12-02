using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NatureQuest.ViewModels
{
    public class ObservationViewModel
    {
        public int ObservationId { get; set; }

        [Required(ErrorMessage = "Species is required.")]
        [Display(Name = "Species Name")]
        public string SpeciesName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Location is required.")]
        [Display(Name = "Location Observed")]
        public string LocationName { get; set; } = string.Empty;

        [Range(-90, 90, ErrorMessage = "Latitude must be between -90 and 90.")]
        [Display(Name = "Latitude")]
        public double? Latitude { get; set; }

        [Range(-180, 180, ErrorMessage = "Longitude must be between -180 and 180.")]
        [Display(Name = "Longitude")]
        public double? Longitude { get; set; }

        [Display(Name = "Date Observed")]
        [DataType(DataType.Date)]
        public DateTime DateObserved { get; set; } = DateTime.Now;

        [Display(Name = "Notes")]
        public string? Notes { get; set; }

        [Display(Name = "Image URL")]
        public string? ImagePath { get; set; }

        public bool HasImage => !string.IsNullOrEmpty(ImagePath);

        public IEnumerable<SelectListItem>? SpeciesList { get; set; }
        public IEnumerable<SelectListItem>? LocationList { get; set; }
    }
}
