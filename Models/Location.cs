using System.ComponentModel.DataAnnotations;

namespace NatureQuest.Models
{
    public class Location
    {
        public int Id { get; set; }

        [Required]
        public string LocationName { get; set; } = string.Empty;

        public string? Region { get; set; }

        public double? Latitude { get; set; } 
        public double? Longitude { get; set; }
    }
}
