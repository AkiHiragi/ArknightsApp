namespace ArknightsApp.Models;

public class OperatorGrowthTemplate
{
    public int      Id         { get; set; }
    public int      OperatorId { get; set; }
    public Operator Operator   { get; set; } = null!;
    
    // Базовые и максимальные значения для каждой элиты
    public int E0BaseHealth     { get; set; }
    public int E0BaseAttack     { get; set; }
    public int E0BaseDefense    { get; set; }
    public int E0BaseResistance { get; set; }
    public int E0MaxHealth      { get; set; }
    public int E0MaxAttack      { get; set; }
    public int E0MaxDefense     { get; set; }
    public int E0MaxResistance  { get; set; }
    
    public int E1BaseHealth     { get; set; }
    public int E1BaseAttack     { get; set; }
    public int E1BaseDefense    { get; set; }
    public int E1BaseResistance { get; set; }
    public int E1MaxHealth      { get; set; }
    public int E1MaxAttack      { get; set; }
    public int E1MaxDefense     { get; set; }
    public int E1MaxResistance  { get; set; }
    
    public int E2BaseHealth     { get; set; }
    public int E2BaseAttack     { get; set; }
    public int E2BaseDefense    { get; set; }
    public int E2BaseResistance { get; set; }
    public int E2MaxHealth      { get; set; }
    public int E2MaxAttack      { get; set; }
    public int E2MaxDefense     { get; set; }
    public int E2MaxResistance  { get; set; }
    
    // Траст бонусы (максимальные значения)
    public int TrustBonusHealth     { get; set; }
    public int TrustBonusAttack     { get; set; }
    public int TrustBonusDefense    { get; set; }
    public int TrustBonusResistance { get; set; }
    
    // Статичные параметры
    public decimal AttackSpeed    { get; set; }
    public string  AttackType     { get; set; } = string.Empty;
    public int     BlockCount     { get; set; }
    public int     DeploymentCost { get; set; }
    public int     RedeployTime   { get; set; }
}
