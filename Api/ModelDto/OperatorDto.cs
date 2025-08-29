using System.ComponentModel.DataAnnotations;

namespace ArknightsApp.ModelDto;

public class OperatorDto
{
    public int Id { get; set; }
    [Required] [MaxLength(100)] public string Name { get; set; } = string.Empty;
    [Range(1, 6)] public int Rarity { get; set; }

    public int ClassId { get; set; }
    public string ClassName { get; set; } = string.Empty;
    public int SubclassId { get; set; }
    public string SubclassName { get; set; } = string.Empty;

    [MaxLength(500)] public string Description { get; set; } = string.Empty;

    public string? AvatarUrl { get; set; }
    public string? PreviewUrl { get; set; }
    public string? E0ArtUrl { get; set; }
    public string? E2ArtUrl { get; set; }
}