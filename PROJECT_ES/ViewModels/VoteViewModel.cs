using System.Collections.Generic;
using PROJECT_ES.Data;

namespace PROJECT_ES.ViewModels
{
    public class VoteViewModel
    {
        public Competition Competition { get; set; }
        public Category Category { get; set; }
        
        public IEnumerable<Movie> Movies { get; set; }
        
      
    }
}