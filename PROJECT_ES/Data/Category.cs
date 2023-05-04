using System.ComponentModel.DataAnnotations;

namespace PROJECT_ES.Data;

public class Category
{
    [Required]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
}