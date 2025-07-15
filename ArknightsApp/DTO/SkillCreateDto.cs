namespace ArknightsApp.DTO;

public class SkillCreateDto
{
    public required string                    Name            { get; set; }
    public          string                    BaseDescription { get; set; } = string.Empty;
    public          string                    IconUrl         { get; set; } = string.Empty;
    public          int                       OperatorId      { get; set; }
    public          List<SkillLevelCreateDto> Levels          { get; set; } = [];
}

public class SkillLevelCreateDto
{
    public int                        Level      { get; set; }
    public string                     LevelName  { get; set; } = string.Empty;
    public int                        SpCost     { get; set; }
    public int                        Duration   { get; set; }
    public Dictionary<string, object> Parameters { get; set; } = [];
}