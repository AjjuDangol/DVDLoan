using System.ComponentModel.DataAnnotations;

namespace Coursework.Models;

public class User
{
    [Key]
    public int UserNumber { get; set; }
    [Required]
    public string UserName { get; set; }
    [Required]
    public string UserType { get; set; }
    [Required]
    public string UserPassword { get; set; }
}