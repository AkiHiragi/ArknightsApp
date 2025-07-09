using ArknightsApp.DTO;
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

    public async Task<IEnumerable<Operator>> SearchByNameAsync(string name)
    {
        return await _context.Operators
                             .Include(o => o.OperatorClass)
                             .Include(o => o.SubClass)
                             .Include(o => o.Faction)
                             .Where(o => o.Name.Contains(name))
                             .ToListAsync();
    }

    public async Task<PagedResult<Operator>> GetPagedAsync(int page, int pageSize)
    {
        var totalCount = await _context.Operators.CountAsync();

        var operators = await _context.Operators
                                      .Include(o => o.OperatorClass)
                                      .Include(o => o.SubClass)
                                      .Include(o => o.Faction)
                                      .OrderBy(o => o.Name)
                                      .Skip((page - 1) * pageSize)
                                      .Take(pageSize)
                                      .ToListAsync();
        
        return new PagedResult<Operator>
        {
            Items      = operators,
            TotalCount = totalCount,
            Page       = page,
            PageSize   = pageSize
        };
    }

    public async Task<PagedResult<Operator>> SearchAsync(SearchRequest request)
    {
        var query = _context.Operators
                            .Include(o => o.OperatorClass)
                            .Include(o => o.SubClass)
                            .Include(o => o.Faction)
                            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Name))
            query = query.Where(o => o.Name.Contains(request.Name));

        if (request.Rarity.HasValue)
            query = query.Where(o => o.Rarity == request.Rarity.Value);

        if (!string.IsNullOrWhiteSpace(request.ClassName))
            query = query.Where(o => o.OperatorClass!.Name.Contains(request.ClassName));

        if (!string.IsNullOrWhiteSpace(request.FactionName))
            query = query.Where(o => o.Faction != null && o.Faction.Name.Contains(request.FactionName));

        query = request.SortBy.ToLower() switch
        {
            "rarity" => request.SortDescending
                            ? query.OrderByDescending(o => o.Rarity)
                            : query.OrderBy(o => o.Rarity),
            "name" => request.SortDescending
                          ? query.OrderByDescending(o => o.Name)
                          : query.OrderBy(o => o.Name),
            _ => request.SortDescending
                     ? query.OrderByDescending(o => o.CnReleaseDate)
                     : query.OrderBy(o => o.CnReleaseDate)
        };

        var totalCount = await query.CountAsync();

        var operators = await query
                             .Skip((request.Page - 1) * request.PageSize)
                             .Take(request.PageSize)
                             .ToListAsync();

        return new PagedResult<Operator>
        {
            Items      = operators,
            TotalCount = totalCount,
            Page       = request.Page,
            PageSize   = request.PageSize
        };
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Operators.AnyAsync(o => o.Id == id);
    }
}