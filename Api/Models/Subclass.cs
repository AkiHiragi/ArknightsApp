using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ArknightsApp.Models;

public class Subclass
{
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(200)]
    public string Description { get; set; } = string.Empty;

    // Связь с классом
    public int ClassId { get; set; }
    public Class Class { get; set; } = null!;

    // Связь с операторами
    public ICollection<Operator> Operators { get; set; } = new List<Operator>();
}