namespace ArknightsApp.Models;

public class TalentLevel
{
    public int                        Id              { get; set; }
    public int                        TalentId        { get; set; }
    public Talent                     Talent          { get; set; } = null!;
    public int                        PotentialLevel  { get; set; }
    public string                     UnlockCondition { get; set; } = string.Empty;
    public Dictionary<string, object> Parameters      { get; set; } = [];
}