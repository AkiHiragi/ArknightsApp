namespace ArknightsApp.Models;

public class SubClass
{
    public          int    Id              { get; set; }
    public required string Name            { get; set; }
    public          string Description     { get; set; } = string.Empty;
    public          int    OperatorClassId { get; set; }

    public OperatorClass? OperatorClass { get; set; }
    public List<Operator> Operators     { get; set; } = [];
}