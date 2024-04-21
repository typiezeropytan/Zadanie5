using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models;

public class Animal
{
    public int IDANIMAL { get; set; }
    [Required]
    [MaxLength(200)]
    public string NAME { get; set; }
    [Required]
    [MaxLength(2000)]
    public string DESCRIPTION { get; set; }
    [Required]
    [MaxLength(200)]
    public string CATEGORY { get; set; }
    [Required]
    [MaxLength(200)]
    public string AREA { get; set; }

}