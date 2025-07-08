namespace ArknightsApp.Models;

public class ModuleLevel
{
    public          int                        Id             { get; set; }
    public          int                        ModuleId       { get; set; }
    public required Module                     Module         { get; set; }
    public          int                        Level          { get; set; }
    public          Dictionary<string, object> StatBonuses    { get; set; } = [];
    public          List<ModuleTalentUpgrade>  TalentUpgrades { get; set; } = [];
}