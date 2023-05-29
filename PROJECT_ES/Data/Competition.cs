using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace PROJECT_ES.Data
{
    public class Competition
    {
        public int Id { get; set; }
        public String Description { get; set; }
        public string Name { get; set; }
        public DateTime data_inicio { get; set; }
        public DateTime data_fim { get; set; }
        public int n_participantes { get; set; }
        public bool Ispublic { get; set; }
        public ICollection<Competition_has_Movie> Competition_has_Movies { get; set; }
        
        public ICollection<Competition_has_Category> Competition_has_Category { get; set; }
        
        public string Image { get; set; }

    }
}
