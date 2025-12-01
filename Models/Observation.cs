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
        public string Location { get; set; } = string.Empty;

        [Display(Name = "Date Observed")]
        [DataType(DataType.Date)]
        public DateTime DateObserved { get; set; } = DateTime.Now;

        public string? Notes { get; set; }

        public string? ImagePath { get; set; } // optional for photos
    }
}
