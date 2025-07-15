namespace ArknightsApp.DTO;

public class TalentCreateDto
{
    public required string                     Name            { get; set; }
    public          string                     BaseDescription { get; set; } = string.Empty;
    public          string                     IconUrl         { get; set; } = string.Empty;
    public          int                        OperatorId      { get; set; }
    public          List<TalentLevelCreateDto> Levels          { get; set; } = [];
}

public class TalentLevelCreateDto
{
    public int                        PotentialLevel  { get; set; }
    public string                     UnlockCondition { get; set; } = string.Empty;
    public Dictionary<string, object> Parameters      { get; set; } = [];
}