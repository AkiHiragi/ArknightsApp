namespace ArknightsApp.DTO;

public class OperatorCreateDto
{
    public string Name            { get; set; } = string.Empty;
    public int    Rarity          { get; set; }
    public int    OperatorClassId { get; set; }
    public int    SubClassId      { get; set; }
    public int?   FactionId       { get; set; }
    public string ImageUrl        { get; set; } = string.Empty;
    public string Description     { get; set; } = string.Empty;
    public string Position        { get; set; } = string.Empty;

    public DateTime  CnReleaseDate     { get; set; }
    public DateTime? GlobalReleaseDate { get; set; }
}

public class OperatorCreateFormDto
{
    public OperatorCreateDto      Operator           { get; set; } = new();
    public List<OperatorClassDto> AvailableClasses   { get; set; } = [];
    public List<FactionDto>       AvailableFactions  { get; set; } = [];
    public List<string>           AvailablePositions { get; set; } = ["Melee", "Ranged"];
    public List<int>              AvailableRarities  { get; set; } = [1, 2, 3, 4, 5, 6];
}