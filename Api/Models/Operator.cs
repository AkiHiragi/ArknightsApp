using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ArknightsApp.Models;

public class Operator
{
    public int Id { get; set; }

    [Required] [MaxLength(100)] public string Name { get; set; } = string.Empty;

    [Range(1, 6)] public int Rarity { get; set; }

    // Связи с классом и подклассом
    public int ClassId { get; set; }
    public Class Class { get; set; } = null!;

    public int SubclassId { get; set; }
    public Subclass Subclass { get; set; } = null!;

    [MaxLength(500)] public string Description { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Изображения
    public string? AvatarUrl { get; set; }
    public string? PreviewUrl { get; set; }
    public string? E0ArtUrl { get; set; }
    public string? E2ArtUrl { get; set; }

    // Скины
    public ICollection<Skin> Skins { get; set; } = new List<Skin>();
}