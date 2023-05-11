using System.Collections.Generic;
using PROJECT_ES.Data;

namespace PROJECT_ES.ViewModels
{
    public class CategoryViewModel
    {
        public Competition Competition { get; set; }
        public Category Category { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Movie> Movies { get; set; }
        
      
    }
}