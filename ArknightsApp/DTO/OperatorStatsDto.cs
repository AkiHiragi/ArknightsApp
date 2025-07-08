namespace ArknightsApp.DTO;

public class OperatorStatsDto
{
    public int Level      { get; set; }
    public int Elite      { get; set; }
    public int TrustLevel { get; set; }
        
    // Основные характеристики
    public int Health     { get; set; }
    public int Attack     { get; set; }
    public int Defense    { get; set; }
    public int Resistance { get; set; }
        
    // Боевые параметры
    public decimal AttackSpeed    { get; set; }
    public string  AttackType     { get; set; } = string.Empty;
    public int     BlockCount     { get; set; }
    public int     DeploymentCost { get; set; }
    public int     RedeployTime   { get; set; }
}