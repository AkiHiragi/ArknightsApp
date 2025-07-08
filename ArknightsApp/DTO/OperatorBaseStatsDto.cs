namespace ArknightsApp.DTO;

public class OperatorBaseStatsDto
{
    public decimal AttackSpeed    { get; set; }
    public string  AttackType     { get; set; } = string.Empty;
    public int     BlockCount     { get; set; }
    public int     DeploymentCost { get; set; }
    public int     RedeployTime   { get; set; }
    
    public int  MaxE0Level { get; set; }
    public int? MaxE1Level { get; set; }
    public int? MaxE2Level { get; set; }
}