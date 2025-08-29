using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ArknightsApp.Models;

public class Class
{
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(200)]
    public string Description { get; set; } = string.Empty;

    // Связи
    public ICollection<Subclass> Subclasses { get; set; } = new List<Subclass>();
    public ICollection<Operator> Operators { get; set; } = new List<Operator>();
}