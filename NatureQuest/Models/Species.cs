using System.ComponentModel.DataAnnotations;

namespace NatureQuest.Models
{
    public class Species
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Common Name")]
        public string CommonName { get; set; } = string.Empty;

        [Display(Name = "Scientific Name")]
        public string? ScientificName { get; set; }

        public string? Category { get; set; } // e.g., Bird, Mammal, Plant, etc.
    }
}
