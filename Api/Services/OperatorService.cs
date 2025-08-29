using System.Collections.Generic;
using System.Threading.Tasks;
using ArknightsApp.Data;
using ArknightsApp.ModelDto;
using ArknightsApp.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ArknightsApp.Services;

public class OperatorService(ApplicationDbContext context, IMapper mapper) : IOperatorService
{
    public async Task<IEnumerable<OperatorDto>> GetAllAsync()
    {
        var operators = await context.Operators
                                     .AsNoTracking()
                                     .Include(o => o.Class)
                                     .Include(o => o.Subclass)
                                     .ToListAsync();

        return mapper.Map<IEnumerable<OperatorDto>>(operators);
    }

    public async Task<OperatorDto?> GetByIdAsync(int id)
    {
        var op = await context.Operators
                              .AsNoTracking()
                              .Include(o => o.Class)
                              .Include(o => o.Subclass)
                              .FirstOrDefaultAsync(o => o.Id == id);

        return mapper.Map<OperatorDto>(op);
    }

    public async Task<OperatorDto> CreateAsync(OperatorDto dto)
    {
        var op = mapper.Map<Operator>(dto);

        context.Operators.Add(op);
        await context.SaveChangesAsync();

        var created = await context.Operators
                                   .Include(o => o.Class)
                                   .Include(o => o.Subclass)
                                   .FirstAsync(o => o.Id == op.Id);

        return mapper.Map<OperatorDto>(created);
    }

    public async Task<OperatorDto?> UpdateAsync(int id, OperatorDto dto)
    {
        var op = await context.Operators.FindAsync(id);
        if (op == null) return null;

        mapper.Map(dto, op);
        await context.SaveChangesAsync();

        var updated = await context.Operators
                                   .Include(o => o.Class)
                                   .Include(o => o.Subclass)
                                   .FirstAsync(o => o.Id == id);

        return mapper.Map<OperatorDto>(updated);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var op = await context.Operators.FindAsync(id);
        if (op == null) return false;
        context.Operators.Remove(op);
        await context.SaveChangesAsync();
        return true;
    }
}