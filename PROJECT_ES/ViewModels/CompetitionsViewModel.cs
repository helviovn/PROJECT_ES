using System.Collections.Generic;
using PROJECT_ES.Data;

namespace PROJECT_ES.ViewModels
{
    public class CompetitionsViewModel
    {
        public Competition Competition { get; set; }
        public IEnumerable<Category> Category { get; set; }
        public IEnumerable<Movie> Movies { get; set; }
        public IEnumerable<Vote> statistics { get; set; }
        
    }
}