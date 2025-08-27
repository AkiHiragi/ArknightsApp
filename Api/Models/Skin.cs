using System.ComponentModel.DataAnnotations;

namespace ArknightsApp.Models;

public class Skin
{
    public int Id { get; set; }
    [Required] [MaxLength(100)] public string Name { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public int OperatorId { get; set; }
    public Operator Operator { get; set; } = null!;
}