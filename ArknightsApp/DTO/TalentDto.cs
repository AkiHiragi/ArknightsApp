namespace ArknightsApp.DTO;

public class TalentDto
{
    public int                  Id          { get; set; }
    public string               Name        { get; set; } = string.Empty;
    public string               Description { get; set; } = string.Empty;
    public string               IconUrl     { get; set; } = string.Empty;
    public List<TalentLevelDto> Levels      { get; set; } = [];
}