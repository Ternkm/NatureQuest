using System.Collections.Generic;
using NatureQuest.Models;

namespace NatureQuest.Models.ViewModels
{
    public class SpeciesViewModel
    {
        public IEnumerable<Species>? SpeciesList { get; set; }
        public Species? Species { get; set; }
    }
}
