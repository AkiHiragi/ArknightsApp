using ArknightsApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ArknightsApp.Data;

public class DbSeeder
{
    public static async Task SeedAsync(ArknightsDbContext context)
    {
        await context.Database.EnsureCreatedAsync();

        if (!await context.Users.AnyAsync())
        {
            var admin = new User
            {
                Username     = "admin",
                Email        = "admin@arknights.local",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                Role         = UserRole.Admin,
                CreatedAt    = DateTime.UtcNow,
                IsActive     = true
            };

            context.Users.Add(admin);
            await context.SaveChangesAsync();
        }

        if (await context.Operators.AnyAsync())
        {
            return;
        }

        var operatorClasses = new List<OperatorClass>
        {
            new()
            {
                Id = 1, Name = "Guard", Description = "Ближний бой, блокирует врагов", IconUrl = "/icons/guard.png"
            },
            new()
            {
                Id = 2, Name = "Caster", Description = "Магический урон на расстоянии", IconUrl = "/icons/caster.png"
            },
            new()
            {
                Id = 3, Name = "Sniper", Description = "Физический урон на расстоянии", IconUrl = "/icons/sniper.png"
            },
            new()
            {
                Id      = 4, Name = "Defender", Description = "Высокая защита, блокирует врагов",
                IconUrl = "/icons/defender.png"
            },
            new() { Id = 5, Name = "Medic", Description     = "Лечит союзников", IconUrl     = "/icons/medic.png" },
            new() { Id = 6, Name = "Supporter", Description = "Поддержка и дебаффы", IconUrl = "/icons/supporter.png" },
            new()
            {
                Id = 7, Name = "Vanguard", Description = "Генерирует DP, первая линия", IconUrl = "/icons/vanguard.png"
            },
            new()
            {
                Id = 8, Name = "Specialist", Description = "Уникальные способности", IconUrl = "/icons/specialist.png"
            }
        };

        context.OperatorClasses.AddRange(operatorClasses);
        await context.SaveChangesAsync();

        var subClasses = new List<SubClass>
        {
            new() { Id = 1, Name = "Arts Guard", OperatorClassId      = 1 },
            new() { Id = 2, Name = "Duelist Guard", OperatorClassId   = 1 },
            new() { Id = 3, Name = "Lord Guard", OperatorClassId      = 1 },
            new() { Id = 4, Name = "Core Caster", OperatorClassId     = 2 },
            new() { Id = 5, Name = "Splash Caster", OperatorClassId   = 2 },
            new() { Id = 6, Name = "Chain Caster", OperatorClassId    = 2 },
            new() { Id = 7, Name = "Marksman Sniper", OperatorClassId = 3 },
            new() { Id = 8, Name = "Spreadshooter", OperatorClassId   = 3 },
            new() { Id = 9, Name = "Deadeye Sniper", OperatorClassId  = 3 }
        };

        context.SubClasses.AddRange(subClasses);
        await context.SaveChangesAsync();

        // Создаем фракции
        var factions = new List<Faction>
        {
            new()
            {
                Id      = 1, Name = "Rhodes Island", Description = "Главная фармацевтическая компания",
                LogoUrl = "/logos/rhodes.png"
            },
            new() { Id = 2, Name = "Lungmen", Description = "Город-государство", LogoUrl = "/logos/lungmen.png" },
            new()
            {
                Id      = 3, Name = "Penguin Logistics", Description = "Логистическая компания",
                LogoUrl = "/logos/penguin.png"
            },
            new() { Id = 4, Name = "Karlan Trade", Description = "Торговая компания", LogoUrl = "/logos/karlan.png" },
            new() { Id = 5, Name = "Blacksteel", Description   = "Военная компания", LogoUrl = "/logos/blacksteel.png" }
        };

        context.Factions.AddRange(factions);
        await context.SaveChangesAsync();

        // Создаем операторов
        var operators = new List<Operator>
        {
            new()
            {
                Id                = 1,
                Name              = "Amiya",
                Rarity            = 5,
                OperatorClassId   = 2,
                SubClassId        = 4,
                FactionId         = 1,
                ImageUrl          = "/images/operators/amiya.jpg",
                Description       = "Лидер Rhodes Island и талантливый кастер",
                Position          = "Ranged",
                CnReleaseDate     = new DateTime(2019, 4, 30),
                GlobalReleaseDate = new DateTime(2020, 1, 16)
            },
            new()
            {
                Id                = 2,
                Name              = "SilverAsh",
                Rarity            = 6,
                OperatorClassId   = 1,
                SubClassId        = 3,
                FactionId         = 4,
                ImageUrl          = "/images/operators/silverash.jpg",
                Description       = "Глава Karlan Trade, мастер меча",
                Position          = "Melee",
                CnReleaseDate     = new DateTime(2019, 4, 30),
                GlobalReleaseDate = new DateTime(2020, 1, 16)
            },
            new()
            {
                Id                = 3,
                Name              = "Exusiai",
                Rarity            = 6,
                OperatorClassId   = 3,
                SubClassId        = 7,
                FactionId         = 3,
                ImageUrl          = "/images/operators/exusiai.jpg",
                Description       = "Снайпер Penguin Logistics с высокой скорострельностью",
                Position          = "Ranged",
                CnReleaseDate     = new DateTime(2019, 4, 30),
                GlobalReleaseDate = new DateTime(2020, 1, 16)
            },
            new()
            {
                Id                = 4,
                Name              = "Eyjafjalla",
                Rarity            = 6,
                OperatorClassId   = 2,
                SubClassId        = 4,
                FactionId         = 1,
                ImageUrl          = "/images/operators/eyjafjalla.jpg",
                Description       = "Мощный кастер с вулканическими способностями",
                Position          = "Ranged",
                CnReleaseDate     = new DateTime(2019, 8, 1),
                GlobalReleaseDate = new DateTime(2020, 3, 25)
            },
            new()
            {
                Id                = 5,
                Name              = "Blaze",
                Rarity            = 6,
                OperatorClassId   = 1,
                SubClassId        = 2,
                FactionId         = 1,
                ImageUrl          = "/images/operators/blaze.jpg",
                Description       = "Элитный оператор Rhodes Island с цепной пилой",
                Position          = "Melee",
                CnReleaseDate     = new DateTime(2020, 4, 29),
                GlobalReleaseDate = new DateTime(2020, 7, 31)
            }
        };

        context.Operators.AddRange(operators);
        await context.SaveChangesAsync();
    }
}