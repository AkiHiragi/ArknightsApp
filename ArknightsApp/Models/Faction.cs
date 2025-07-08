namespace ArknightsApp.Models;

public class Faction
{
    public          int            Id          { get; set; }
    public required string         Name        { get; set; }
    public          string         LogoUrl     { get; set; } = string.Empty;
    public          string         Description { get; set; } = string.Empty;
    public          List<Operator> Operators   { get; set; } = [];
}