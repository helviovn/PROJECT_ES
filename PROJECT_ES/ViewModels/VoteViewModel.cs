using System.Collections.Generic;
using PROJECT_ES.Data;

namespace PROJECT_ES.ViewModels
{
    public class VoteViewModel
    {
        public Competition Competition { get; set; }
        public Category Category { get; set; }
        public IEnumerable<Movie> Movies { get; set; }
        
        
        public int CompetitionId { get; set; }
        public int CategoryId { get; set; }
        public int MovieId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}