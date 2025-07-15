using ArknightsApp.Models;

namespace ArknightsApp.DTO;

public class SkillCreateWithTemplateDto
{
    public required string        Name       { get; set; }
    public          string        IconUrl    { get; set; } = string.Empty;
    public          int           OperatorId { get; set; }
    public required SkillTemplate Template   { get; set; }
}