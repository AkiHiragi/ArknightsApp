namespace ArknightsApp.Models;

public class ModuleTalentUpgrade
{
    public int                        Id                 { get; set; }
    public int                        ModuleLevelId      { get; set; }
    public ModuleLevel                ModuleLevel        { get; set; } = null!;
    public int                        TalentId           { get; set; }
    public Talent                     Talent             { get; set; } = null!;
    public Dictionary<string, object> UpgradeParameters  { get; set; } = [];
    public string                     UpgradeDescription { get; set; } = string.Empty;
}