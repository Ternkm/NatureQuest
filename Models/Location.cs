using System.ComponentModel.DataAnnotations;

namespace NatureQuest.Models
{
    public class Location
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string? Region { get; set; }

        public string? Coordinates { get; set; } // optional for map use
    }
}
