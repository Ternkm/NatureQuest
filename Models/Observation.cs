using System;
using System.ComponentModel.DataAnnotations;

namespace NatureQuest.Models
{
    public class Observation
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Species Name")]
        public string SpeciesName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Location Observed")]
        public string LocationName { get; set; } = string.Empty;
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        [Display(Name = "Date Observed")]
        [DataType(DataType.Date)]
        public DateTime DateObserved { get; set; } = DateTime.Now;

        public string? Notes { get; set; }

        public string? ImagePath { get; set; } // optional for photos
    }
}
