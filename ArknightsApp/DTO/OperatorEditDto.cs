namespace ArknightsApp.DTO;

public class OperatorEditDto
{
    public int       Id                { get; set; }
    public string    Name              { get; set; } = string.Empty;
    public int       Rarity            { get; set; }
    public int       OperatorClassId   { get; set; }
    public int       SubClassId        { get; set; }
    public int?      FactionId         { get; set; }
    public string    ImageUrl          { get; set; } = string.Empty;
    public string    Description       { get; set; } = string.Empty;
    public string    Position          { get; set; } = string.Empty;
    public DateTime  CnReleaseDate     { get; set; }
    public DateTime? GlobalReleaseDate { get; set; }
}