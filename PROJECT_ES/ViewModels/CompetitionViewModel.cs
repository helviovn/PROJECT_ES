using System.Collections.Generic;
using PROJECT_ES.Data;

namespace PROJECT_ES.ViewModels
{
    public class CompetitionViewModel
    {
        public Competition Competition { get; set; }
        public Category Category { get; set; }
        public IEnumerable<Movie> Movies { get; set; }
        
        public int Id { get; set; }
        public String Description { get; set; }
        public string Name { get; set; }
        public DateTime data_inicio { get; set; }
        public DateTime data_fim { get; set; }
        public int n_participantes { get; set; }
        public bool Ispublic { get; set; }
        public ICollection<Competition_has_Movie> Competition_has_Movies { get; set; }
        
        public ICollection<Competition_has_Category> Competition_has_Category { get; set; }
    }
}