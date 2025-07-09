namespace ArknightsApp.Models;

public class Operator
{
    public          int    Id     { get; set; }
    public required string Name   { get; set; }
    public          int    Rarity { get; set; }

    public int            OperatorClassId { get; set; }
    public OperatorClass? OperatorClass   { get; set; }

    public int       SubClassId { get; set; }
    public SubClass? SubClass   { get; set; }

    public int?     FactionId { get; set; }
    public Faction? Faction   { get; set; } = null!;

    public string ImageUrl    { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Position    { get; set; } = string.Empty;

    public List<Skill>          Skills          { get; set; } = [];
    public List<Talent>         Talents         { get; set; } = [];
    public List<OperatorModule> OperatorModules { get; set; } = [];

    public required DateTime  CnReleaseDate     { get; set; }
    public          DateTime? GlobalReleaseDate { get; set; }

    public OperatorGrowthTemplate? GrowthTemplate { get; set; }
}