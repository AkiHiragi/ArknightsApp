using ArknightsApp.Models;

namespace ArknightsApp.Services;

public static class OperatorStatsCalculator
{
    public static (int e0, int? e1, int? e2) GetLevelCaps(int rarity)
        => rarity switch
        {
            1 or 2 => (30, null, null),
            3      => (40, 55, null),
            4      => (45, 60, 70),
            5      => (50, 70, 80),
            6      => (50, 80, 90),
            _      => throw new ArgumentException($"Invalid rarity: {rarity}")
        };

    public static bool CanPromoteToElite(int rarity, int targetElite)
    {
        var caps = GetLevelCaps(rarity);
        return targetElite switch
        {
            0 => true,
            1 => caps.e1.HasValue,
            2 => caps.e2.HasValue,
            _ => false
        };
    }

    public static OperatorCalculatedStats CalculateStats(
        OperatorGrowthTemplate template,
        int                    level,
        int                    elite,
        int                    trustLevel = 0)
    {
        if (!CanPromoteToElite(template.Operator.Rarity, elite))
            throw new ArgumentException($"Elite {elite} not available for {template.Operator.Rarity}★ operator");

        var caps = GetLevelCaps(template.Operator.Rarity);
        var maxLevel = elite switch
        {
            0 => caps.e0,
            1 => caps.e1!.Value,
            2 => caps.e2!.Value,
            _ => throw new ArgumentException($"Invalid elite: {elite}")
        };

        if (level < 1 || level > maxLevel)
            throw new ArgumentException($"Level {level} not valid for E{elite} (max: {{maxLevel}})");

        var baseStats = GetBaseStatsForElite(template, elite);
        var maxStats  = GetMaxStatsForElite(template, elite);

        var progress = (level - 1) / (double)(maxLevel - 1);

        var health     = (int)(baseStats.Health     + (maxStats.Health     - baseStats.Health)     * progress);
        var attack     = (int)(baseStats.Attack     + (maxStats.Attack     - baseStats.Attack)     * progress);
        var defense    = (int)(baseStats.Defense    + (maxStats.Defense    - baseStats.Defense)    * progress);
        var resistance = (int)(baseStats.Resistance + (maxStats.Resistance - baseStats.Resistance) * progress);

        var trustBonus = CalculateTrustBonus(template, trustLevel);

        return new OperatorCalculatedStats
        {
            Level          = level,
            Elite          = elite,
            TrustLevel     = trustLevel,
            Health         = health     + trustBonus.Health,
            Attack         = attack     + trustBonus.Attack,
            Defense        = defense    + trustBonus.Defense,
            Resistance     = resistance + trustBonus.Resistance,
            AttackSpeed    = template.AttackSpeed,
            AttackType     = template.AttackType,
            BlockCount     = template.BlockCount,
            DeploymentCost = template.DeploymentCost,
            RedeployTime   = template.RedeployTime
        };
    }

    private static TrustBonus CalculateTrustBonus(OperatorGrowthTemplate template, int trustLevel)
    {
        if (trustLevel <= 0) return new TrustBonus();

        var trustProgress = Math.Min(trustLevel, 100) / 100.0;

        return new TrustBonus
        {
            Health     = (int)(template.TrustBonusHealth     * trustProgress),
            Attack     = (int)(template.TrustBonusAttack     * trustProgress),
            Defense    = (int)(template.TrustBonusDefense    * trustProgress),
            Resistance = (int)(template.TrustBonusResistance * trustProgress)
        };
    }

    private static BaseStats GetBaseStatsForElite(OperatorGrowthTemplate template, int elite)
    {
        return elite switch
        {
            0 => new BaseStats
            {
                Health     = template.E0BaseHealth,
                Attack     = template.E0BaseAttack,
                Defense    = template.E0BaseDefense,
                Resistance = template.E0BaseResistance
            },
            1 => new BaseStats
            {
                Health     = template.E1BaseHealth,
                Attack     = template.E1BaseAttack,
                Defense    = template.E1BaseDefense,
                Resistance = template.E1BaseResistance
            },
            2 => new BaseStats
            {
                Health     = template.E2BaseHealth,
                Attack     = template.E2BaseAttack,
                Defense    = template.E2BaseDefense,
                Resistance = template.E2BaseResistance
            },
            _ => throw new ArgumentException($"Invalid elite: {elite}")
        };
    }
    
    private static BaseStats GetMaxStatsForElite(OperatorGrowthTemplate template, int elite)
    {
        return elite switch
        {
            0 => new BaseStats
            {
                Health     = template.E0MaxHealth,
                Attack     = template.E0MaxAttack,
                Defense    = template.E0MaxDefense,
                Resistance = template.E0MaxResistance
            },
            1 => new BaseStats
            {
                Health     = template.E1MaxHealth,
                Attack     = template.E1MaxAttack,
                Defense    = template.E1MaxDefense,
                Resistance = template.E1MaxResistance
            },
            2 => new BaseStats
            {
                Health     = template.E2MaxHealth,
                Attack     = template.E2MaxAttack,
                Defense    = template.E2MaxDefense,
                Resistance = template.E2MaxResistance
            },
            _ => throw new ArgumentException($"Invalid elite: {elite}")
        };
    }

    public class OperatorCalculatedStats
    {
        public int     Level          { get; set; }
        public int     Elite          { get; set; }
        public int     TrustLevel     { get; set; }
        public int     Health         { get; set; }
        public int     Attack         { get; set; }
        public int     Defense        { get; set; }
        public int     Resistance     { get; set; }
        public decimal AttackSpeed    { get; set; }
        public string  AttackType     { get; set; } = string.Empty;
        public int     BlockCount     { get; set; }
        public int     DeploymentCost { get; set; }
        public int     RedeployTime   { get; set; }
    }

    public class BaseStats
    {
        public int Health     { get; set; }
        public int Attack     { get; set; }
        public int Defense    { get; set; }
        public int Resistance { get; set; }
    }

    public class TrustBonus
    {
        public int Health     { get; set; }
        public int Attack     { get; set; }
        public int Defense    { get; set; }
        public int Resistance { get; set; }
    }
}