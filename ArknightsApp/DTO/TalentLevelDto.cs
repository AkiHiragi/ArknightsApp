namespace ArknightsApp.DTO;

public class TalentLevelDto
{
    public int    PotentialLevel  { get; set; }
    public string UnlockCondition { get; set; } = string.Empty; 
    public string Description     { get; set; } = string.Empty;
}