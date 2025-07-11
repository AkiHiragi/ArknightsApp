using ArknightsApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ArknightsApp.Data;

public class ArknightsDbContext : DbContext
{
    public ArknightsDbContext(DbContextOptions<ArknightsDbContext> options) : base(options)
    {
    }

    // Основные сущности
    public DbSet<Operator>      Operators       { get; set; }
    public DbSet<OperatorClass> OperatorClasses { get; set; }
    public DbSet<SubClass>      SubClasses      { get; set; }
    public DbSet<Faction>       Factions        { get; set; }

    // Навыки и таланты
    public DbSet<Skill>       Skills       { get; set; }
    public DbSet<SkillLevel>  SkillLevels  { get; set; }
    public DbSet<Talent>      Talents      { get; set; }
    public DbSet<TalentLevel> TalentLevels { get; set; }

    // Модули
    public DbSet<Module>              Modules              { get; set; }
    public DbSet<ModuleLevel>         ModuleLevels         { get; set; }
    public DbSet<OperatorModule>      OperatorModules      { get; set; }
    public DbSet<ModuleTalentUpgrade> ModuleTalentUpgrades { get; set; }

    // Характеристики
    public DbSet<OperatorGrowthTemplate> OperatorGrowthTemplates { get; set; }

    // Пользователи
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigureOperator(modelBuilder);
        ConfigureSkillsAndTalents(modelBuilder);
        ConfigureModules(modelBuilder);
        ConfigureUsers(modelBuilder);
    }

    private void ConfigureOperator(ModelBuilder modelBuilder)
    {
        // Operator
        modelBuilder.Entity<Operator>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.ImageUrl).HasMaxLength(500);
            entity.Property(e => e.Position).HasMaxLength(20);

            // Связи
            entity.HasOne(e => e.OperatorClass)
                  .WithMany(e => e.Operators)
                  .HasForeignKey(e => e.OperatorClassId);

            entity.HasOne(e => e.SubClass)
                  .WithMany(e => e.Operators)
                  .HasForeignKey(e => e.SubClassId);

            entity.HasOne(e => e.Faction)
                  .WithMany(e => e.Operators)
                  .HasForeignKey(e => e.FactionId)
                  .IsRequired(false);

            entity.HasOne(e => e.GrowthTemplate)
                  .WithOne(e => e.Operator)
                  .HasForeignKey<OperatorGrowthTemplate>(e => e.OperatorId);
        });

        // OperatorClass
        modelBuilder.Entity<OperatorClass>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.IconUrl).HasMaxLength(500);
        });

        // SubClass
        modelBuilder.Entity<SubClass>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Description).HasMaxLength(500);

            entity.HasOne(e => e.OperatorClass)
                  .WithMany(e => e.SubClasses)
                  .HasForeignKey(e => e.OperatorClassId);
        });

        // Faction
        modelBuilder.Entity<Faction>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.LogoUrl).HasMaxLength(500);
        });
    }

    private void ConfigureSkillsAndTalents(ModelBuilder modelBuilder)
    {
        // Skill
        modelBuilder.Entity<Skill>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.BaseDescription).HasMaxLength(1000);
            entity.Property(e => e.IconUrl).HasMaxLength(500);

            entity.HasOne(e => e.Operator)
                  .WithMany(e => e.Skills)
                  .HasForeignKey(e => e.OperatorId);
        });

        // SkillLevel
        modelBuilder.Entity<SkillLevel>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.LevelName).HasMaxLength(50);
            entity.Property(e => e.Parameters).HasConversion(
                v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions)null),
                v => System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(
                    v, (System.Text.Json.JsonSerializerOptions)null));

            entity.HasOne(e => e.Skill)
                  .WithMany(e => e.SkillLevels)
                  .HasForeignKey(e => e.SkillId);
        });

        // Talent
        modelBuilder.Entity<Talent>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.BaseDescription).HasMaxLength(1000);
            entity.Property(e => e.IconUrl).HasMaxLength(500);

            entity.HasOne(e => e.Operator)
                  .WithMany(e => e.Talents)
                  .HasForeignKey(e => e.OperatorId);
        });

        // TalentLevel
        modelBuilder.Entity<TalentLevel>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.UnlockCondition).HasMaxLength(100);
            entity.Property(e => e.Parameters).HasConversion(
                v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions)null),
                v => System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(
                    v, (System.Text.Json.JsonSerializerOptions)null));

            entity.HasOne(e => e.Talent)
                  .WithMany(e => e.TalentLevels)
                  .HasForeignKey(e => e.TalentId);
        });
    }

    private void ConfigureModules(ModelBuilder modelBuilder)
    {
        // Module
        modelBuilder.Entity<Module>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.TypeCode).HasMaxLength(10);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.IconUrl).HasMaxLength(500);
        });

        // ModuleLevel - настраиваем конвертацию Dictionary
        modelBuilder.Entity<ModuleLevel>(entity =>
        {
            entity.HasKey(e => e.Id);

            // Конвертируем Dictionary в JSON строку
            entity.Property(e => e.StatBonuses)
                  .HasConversion(
                       v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions)null),
                       v => System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(
                                v, (System.Text.Json.JsonSerializerOptions)null) ?? new Dictionary<string, object>());

            entity.HasOne(e => e.Module)
                  .WithMany(e => e.ModuleLevels)
                  .HasForeignKey(e => e.ModuleId);
        });

        // ModuleTalentUpgrade - тоже настраиваем Dictionary
        modelBuilder.Entity<ModuleTalentUpgrade>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.UpgradeDescription).HasMaxLength(1000);

            entity.Property(e => e.UpgradeParameters)
                  .HasConversion(
                       v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions)null),
                       v => System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(
                                v, (System.Text.Json.JsonSerializerOptions)null) ?? new Dictionary<string, object>());

            entity.HasOne(e => e.ModuleLevel)
                  .WithMany(e => e.TalentUpgrades)
                  .HasForeignKey(e => e.ModuleLevelId);

            entity.HasOne(e => e.Talent)
                  .WithMany()
                  .HasForeignKey(e => e.TalentId);
        });

        // OperatorModule - связь многие ко многим
        modelBuilder.Entity<OperatorModule>(entity =>
        {
            entity.HasKey(e => new { e.OperatorId, e.ModuleId });

            entity.HasOne(e => e.Operator)
                  .WithMany(e => e.OperatorModules)
                  .HasForeignKey(e => e.OperatorId);

            entity.HasOne(e => e.Module)
                  .WithMany(e => e.OperatorModules)
                  .HasForeignKey(e => e.ModuleId);
        });

        // OperatorGrowthTemplate
        modelBuilder.Entity<OperatorGrowthTemplate>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.AttackType).HasMaxLength(20);

            entity.HasOne(e => e.Operator)
                  .WithOne(e => e.GrowthTemplate)
                  .HasForeignKey<OperatorGrowthTemplate>(e => e.OperatorId);
        });
    }

    private void ConfigureUsers(ModelBuilder modelBuilder)
    {
        // User
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasIndex(e => e.Username).IsUnique();
            entity.HasIndex(e => e.Email).IsUnique();

            entity.Property(e => e.Username)
                  .IsRequired()
                  .HasMaxLength(50);

            entity.Property(e => e.Email)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(e => e.PasswordHash)
                  .IsRequired()
                  .HasMaxLength(255);

            entity.Property(e => e.Role)
                  .HasConversion<string>();
        });
    }
}