namespace ArknightsApp.Models;

public class SkillLevel
{
    public int                        Id         { get; set; }
    public int                        SkillId    { get; set; }
    public Skill?                     Skill      { get; set; }
    public int                        Level      { get; set; } = 0;
    public string                     LevelName  { get; set; } = string.Empty;
    public int                        SpCost     { get; set; }
    public int                        Duration   { get; set; }
    public Dictionary<string, object> Parameters { get; set; } = [];
}