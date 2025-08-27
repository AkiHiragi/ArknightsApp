using System;
using System.ComponentModel.DataAnnotations;

namespace ArknightsApp.Models;

public class Operator
{
    public int Id { get; set; }

    [Required] [MaxLength(100)] public string Name { get; set; } = string.Empty;

    [Range(1, 6)] public int Rarity { get; set; }

    [MaxLength(50)] public string Class { get; set; } = string.Empty;

    [MaxLength(50)] public string Subclass { get; set; } = string.Empty;

    [MaxLength(500)] public string Description { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}