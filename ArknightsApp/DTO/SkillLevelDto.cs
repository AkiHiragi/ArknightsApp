namespace ArknightsApp.DTO;

public class SkillLevelDto
{
    public int    Level       { get; set; }
    public string LevelName   { get; set; } = string.Empty;
    public int    SpCost      { get; set; }
    public int    Duration    { get; set; }
    public string Description { get; set; } = string.Empty;
}