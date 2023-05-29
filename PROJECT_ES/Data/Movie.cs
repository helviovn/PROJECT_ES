using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace PROJECT_ES.Data
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Diretor { get; set; }
        public string Actor { get; set; } 
        public string Genre { get; set; }
        public string Writers { get; set; }
        public string Image { get; set; }
        public string Date { get; set; }
        public string Duration { get; set; }
        public string Country { get; set; }
        public string Rating { get; set; }
        public ICollection<Competition_has_Movie> Competition_has_Movies { get; set; }
    }
}
