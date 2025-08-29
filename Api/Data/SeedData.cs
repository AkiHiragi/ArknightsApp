using System.Threading.Tasks;
using ArknightsApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ArknightsApp.Data;

public class SeedData
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        // Сначала создаем классы
        if (!await context.Classes.AnyAsync())
        {
            var classes = new[]
            {
                new Class { Name = "Guard", Description = "Melee physical damage dealers" },
                new Class { Name = "Sniper", Description = "Ranged physical damage dealers" },
                new Class { Name = "Caster", Description = "Arts damage dealers" },
                new Class { Name = "Defender", Description = "Tank operators" },
                new Class { Name = "Medic", Description = "Healing operators" },
                new Class { Name = "Supporter", Description = "Support operators" },
                new Class { Name = "Vanguard", Description = "DP generation operators" },
                new Class { Name = "Specialist", Description = "Unique role operators" }
            };

            await context.Classes.AddRangeAsync(classes);
            await context.SaveChangesAsync();
        }

        // Затем создаем подклассы
        if (!await context.Subclasses.AnyAsync())
        {
            var guardClass = await context.Classes.FirstAsync(c => c.Name == "Guard");
            var sniperClass = await context.Classes.FirstAsync(c => c.Name == "Sniper");
            var casterClass = await context.Classes.FirstAsync(c => c.Name == "Caster");

            var subclasses = new[]
            {
                new Subclass { Name = "Lord", ClassId = guardClass.Id },
                new Subclass { Name = "Duelist Guard", ClassId = guardClass.Id },
                new Subclass { Name = "Anti-Air Sniper", ClassId = sniperClass.Id },
                new Subclass { Name = "Core Caster", ClassId = casterClass.Id }
            };

            await context.Subclasses.AddRangeAsync(subclasses);
            await context.SaveChangesAsync();
        }

        // Наконец создаем операторов
        if (!await context.Operators.AnyAsync())
        {
            var casterClass = await context.Classes.FirstAsync(c => c.Name == "Caster");
            var sniperClass = await context.Classes.FirstAsync(c => c.Name == "Sniper");
            var guardClass = await context.Classes.FirstAsync(c => c.Name == "Guard");

            var coreCaster = await context.Subclasses.FirstAsync(s => s.Name == "Core Caster");
            var antiAirSniper = await context.Subclasses.FirstAsync(s => s.Name == "Anti-Air Sniper");
            var lord = await context.Subclasses.FirstAsync(s => s.Name == "Lord");
            var duelistGuard = await context.Subclasses.FirstAsync(s => s.Name == "Duelist Guard");

            var operators = new[]
            {
                new Operator
                {
                    Name = "Amiya", Rarity = 5,
                    ClassId = casterClass.Id, SubclassId = coreCaster.Id,
                    Description = "Leader of Rhodes Island"
                },
                new Operator
                {
                    Name = "Exusiai", Rarity = 6,
                    ClassId = sniperClass.Id, SubclassId = antiAirSniper.Id,
                    Description = "Penguin Logistics messenger"
                },
                new Operator
                {
                    Name = "SilverAsh", Rarity = 6,
                    ClassId = guardClass.Id, SubclassId = lord.Id,
                    Description = "Karlan Trade CEO"
                },
                new Operator
                {
                    Name = "Melantha", Rarity = 3,
                    ClassId = guardClass.Id, SubclassId = duelistGuard.Id,
                    Description = "Reliable 3-star guard"
                }
            };

            await context.Operators.AddRangeAsync(operators);
            await context.SaveChangesAsync();
        }
    }
}