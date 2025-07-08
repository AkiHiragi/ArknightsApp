namespace ArknightsApp.Models;

public class OperatorClass
{
    public          int    Id          { get; set; }
    public required string Name        { get; set; }
    public          string Description { get; set; } = string.Empty;
    public          string IconUrl     { get; set; } = string.Empty;

    public List<SubClass> SubClasses { get; set; } = [];
    public List<Operator> Operators  { get; set; } = [];
}