using ArknightsApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ArknightsApp.Data;

public class OperatorRepository : IOperatorRepository
{
    private readonly ArknightsDbContext _context;

    public OperatorRepository(ArknightsDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Operator>> GetAllAsync()
    {
        return await _context.Operators
                             .Include(o => o.OperatorClass)
                             .Include(o => o.SubClass)
                             .Include(o => o.Faction)
                             .ToListAsync();
    }

    public async Task<Operator?> GetByIdAsync(int id)
    {
        return await _context.Operators
                             .Include(o => o.OperatorClass)
                             .Include(o => o.SubClass)
                             .Include(o => o.Faction)
                             .Include(o => o.Skills)
                             .Include(o => o.Talents)
                             .Include(o => o.GrowthTemplate)
                             .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<Operator> CreateAsync(Operator op)
    {
        _context.Operators.Add(op);
        await _context.SaveChangesAsync();
        return op;
    }

    public async Task<Operator?> UpdateAsync(Operator op)
    {
        _context.Entry(op).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return op;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var op = await _context.Operators.FindAsync(id);
        if (op == null) return false;

        _context.Operators.Remove(op);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<Operator>> GetByRarityAsync(int rarity)
    {
        return await _context.Operators
                             .Include(o => o.OperatorClass)
                             .Include(o => o.SubClass)
                             .Include(o => o.Faction)
                             .Where(o => o.Rarity == rarity)
                             .ToListAsync();
    }

    public async Task<IEnumerable<Operator>> GetByFactionAsync(string faction)
    {
        return await _context.Operators
                             .Include(o => o.OperatorClass)
                             .Include(o => o.SubClass)
                             .Include(o => o.Faction)
                             .Where(o => o.Faction != null && o.Faction.Name.Contains(faction))
                             .ToListAsync();
    }

    public async Task<IEnumerable<Operator>> GetByClassAsync(string className)
    {
        return await _context.Operators
                             .Include(o => o.OperatorClass)
                             .Include(o => o.SubClass)
                             .Include(o => o.Faction)
                             .Where(o => o.OperatorClass.Name.Contains(className))
                             .ToListAsync();
    }

    public async Task<IEnumerable<Operator>> GetNewestAsync(int count)
    {
        return await _context.Operators
                             .Include(o => o.OperatorClass)
                             .Include(o => o.SubClass)
                             .Include(o => o.Faction)
                             .OrderByDescending(o => o.CnReleaseDate)
                             .Take(count)
                             .ToListAsync();
    }

    public async Task<IEnumerable<Operator>> GetGlobalReleasedAsync()
    {
        return await _context.Operators
                             .Include(o => o.OperatorClass)
                             .Include(o => o.SubClass)
                             .Include(o => o.Faction)
                             .Where(o => o.GlobalReleaseDate.HasValue)
                             .ToListAsync();
    }

    public async Task<IEnumerable<Operator>> GetCnOnlyAsync()
    {
        return await _context.Operators
                             .Include(o => o.OperatorClass)
                             .Include(o => o.SubClass)
                             .Include(o => o.Faction)
                             .Where(o => !o.GlobalReleaseDate.HasValue)
                             .ToListAsync();
    }

    public async Task<IEnumerable<Operator>> GetByReleaseDateAsync(DateTime fromDate, DateTime? toDate)
    {
        var query = _context.Operators
                            .Include(o => o.OperatorClass)
                            .Include(o => o.SubClass)
                            .Include(o => o.Faction)
                            .Where(o => o.CnReleaseDate >= fromDate);

        if (toDate.HasValue)
        {
            query = query.Where(o => o.CnReleaseDate <= toDate.Value);
        }

        return await query.ToListAsync();
    }
}