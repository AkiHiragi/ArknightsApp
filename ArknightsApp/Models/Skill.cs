namespace ArknightsApp.Models;

public class Skill
{
    public          int       Id              { get; set; }
    public required string    Name            { get; set; }
    public          string    BaseDescription { get; set; } = string.Empty;
    public          string    IconUrl         { get; set; } = string.Empty;
    public          int       OperatorId      { get; set; }
    public          Operator? Operator        { get; set; } = null!;

    public List<SkillLevel> SkillLevels { get; set; } = [];
}