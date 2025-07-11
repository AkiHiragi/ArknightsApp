namespace ArknightsApp.DTO;

public class OperatorDetailDto
{
    public int     Id           { get; set; }
    public string  Name         { get; set; } = string.Empty;
    public int     Rarity       { get; set; }
    public string  ClassName    { get; set; } = string.Empty;
    public string  SubClassName { get; set; } = string.Empty;
    public string? FactionName  { get; set; }
    public string  ImageUrl     { get; set; } = string.Empty;
    public string  Description  { get; set; } = string.Empty;
    public string  Position     { get; set; } = string.Empty;

    public DateTime  CnReleaseDate     { get; set; }
    public DateTime? GlobalReleaseDate { get; set; }

    public bool   IsGlobalReleased => GlobalReleaseDate.HasValue;
    public string ReleaseStatus    => IsGlobalReleased ? "Global" : "CN Only";

    public List<SkillDto>  Skills  { get; set; } = [];
    public List<TalentDto> Talents { get; set; } = [];

    public OperatorBaseStatsDto BaseStats { get; set; } = new();
}