using System.ComponentModel.DataAnnotations;

namespace PROJECT_ES.Data;

public class User
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    public int Age { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
    public bool AdminRole { get; set; }
}