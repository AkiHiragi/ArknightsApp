namespace ArknightsApp.Models;

public class OperatorModule
{
    public          int      OperatorId { get; set; }
    public required Operator Operator   { get; set; }
    public          int      ModuleId   { get; set; }
    public required Module   Module     { get; set; }
    public          bool     IsUnlocked { get; set; } = false;
}