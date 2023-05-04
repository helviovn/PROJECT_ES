using System.ComponentModel.DataAnnotations.Schema;

namespace PROJECT_ES.Data;

public class Competition_has_Category
{
    [ForeignKey("Competition")]
    public int CompetitionId { get; set; }
    [ForeignKey("Category")]
    public int CategoryId { get; set; }
    public virtual Competition Competition { get; set; }
    public virtual Category Category { get; set; }
}