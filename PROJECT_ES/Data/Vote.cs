using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PROJECT_ES.Data;

public class Vote
{
    [Required]
    public int Id { get; set; }
    [ForeignKey("Movie")]
    public int MovieId { get; set; }
    [ForeignKey("Category")]
    public int CategoryId { get; set; }
    [ForeignKey("Competition")]
    public int CompetitionId { get; set; }
    public string Username { get; set; }
    public string Email { get; set; } 
}