namespace ArknightsApp.Models;

public class Module
{
    public int    Id          { get; set; }
    public string Name        { get; set; } = string.Empty;
    public string TypeCode    { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string IconUrl     { get; set; } = string.Empty;
    
    public List<OperatorModule> OperatorModules { get; set; } = [];
    public List<ModuleLevel>    ModuleLevels    { get; set; } = [];
}