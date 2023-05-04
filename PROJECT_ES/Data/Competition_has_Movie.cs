using System.ComponentModel.DataAnnotations.Schema;

namespace PROJECT_ES.Data;

public class Competition_has_Movie
{
    [ForeignKey("Competition")]
    public int CompetitionId { get; set; }
    [ForeignKey("Movie")]
    public Guid MovieId { get; set; }
    public virtual Competition Competition { get; set; }
    public virtual Movie Movie { get; set; }
}