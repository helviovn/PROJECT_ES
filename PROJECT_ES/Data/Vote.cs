using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PROJECT_ES.Data;

public class Vote
{
    [Required]
    public int Id { get; set; }
    [ForeignKey("User")]
    public string UserId { get; set; }
    [ForeignKey("Movie")]
    public Guid MovieId { get; set; }
    public virtual User User { get; set; }
    public virtual Movie Movie { get; set; }
}