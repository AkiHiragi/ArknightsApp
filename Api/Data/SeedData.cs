using System.Threading.Tasks;
using ArknightsApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ArknightsApp.Data;

public class SeedData
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        if (await context.Operators.AnyAsync()) return;

        var operators = new[]
        {
            new Operator
            {
                Name = "Amiya", Rarity = 5, Class = "Caster", Subclass = "Core Caster",
                Description = "Leader of Rhodes Island"
            },
            new Operator
            {
                Name = "Exusiai", Rarity = 6, Class = "Sniper", Subclass = "Anti-Air Sniper",
                Description = "Penguin Logistics messenger"
            },
            new Operator
            {
                Name = "SilverAsh", Rarity = 6, Class = "Guard", Subclass = "Ranged Guard",
                Description = "Karlan Trade CEO"
            },
            new Operator
            {
                Name = "Melantha", Rarity = 3, Class = "Guard", Subclass = "Duelist Guard",
                Description = "Reliable 3-star guard"
            }
        };

        await context.Operators.AddRangeAsync(operators);
        await context.SaveChangesAsync();
    }
}