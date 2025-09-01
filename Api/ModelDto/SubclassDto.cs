using System.ComponentModel.DataAnnotations;

namespace ArknightsApp.ModelDto;

public class SubclassDto
{
    public int Id { get; set; }

    [Required] [MaxLength(50)] public string Name { get; set; } = string.Empty;

    [MaxLength(200)] public string Description { get; set; } = string.Empty;

    [Required] public int ClassId { get; set; }
    public string? ClassName { get; set; }
}

public class CreateSubclassDto
{
    [Required] [MaxLength(50)] public string Name { get; set; } = string.Empty;

    [MaxLength(200)] public string Description { get; set; } = string.Empty;
    
    [Required] public int ClassId { get; set; }
}