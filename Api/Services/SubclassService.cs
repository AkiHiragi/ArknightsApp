using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArknightsApp.Data;
using ArknightsApp.ModelDto;
using ArknightsApp.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ArknightsApp.Services;

public class SubclassService(ApplicationDbContext context, IMapper mapper) : ISubclassService
{
    public async Task<IEnumerable<SubclassDto>> GetAllAsync()
    {
        var subclasses = await context.Subclasses
            .Include(s => s.Class)
            .AsNoTracking()
            .ToListAsync();

        return mapper.Map<IEnumerable<SubclassDto>>(subclasses);
    }

    public async Task<SubclassDto?> GetByIdAsync(int id)
    {
        var subclass = await context.Subclasses
            .Include(s => s.Class)
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id);

        return subclass == null ? null : mapper.Map<SubclassDto>(subclass);
    }

    public async Task<SubclassDto> CreateAsync([FromBody] CreateSubclassDto dto)
    {
        var subclass = mapper.Map<Subclass>(dto);
        context.Subclasses.Add(subclass);
        await context.SaveChangesAsync();

        await context.Entry(subclass).Reference(s => s.Class).LoadAsync();
        return mapper.Map<SubclassDto>(subclass);
    }

    public async Task<SubclassDto?> UpdateAsync(int id, [FromBody] CreateSubclassDto dto)
    {
        var subclass = await context.Subclasses
            .Include(s => s.Class)
            .FirstOrDefaultAsync(s => s.Id == id);
        if (subclass == null) return null;

        mapper.Map(dto, subclass);
        await context.SaveChangesAsync();
        return mapper.Map<SubclassDto>(subclass);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var subclass = await context.Subclasses.FindAsync(id);
        if (subclass == null) return false;

        context.Subclasses.Remove(subclass);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<OperatorDto>> GetOperatorsBySubclassIdAsync(int subclassId)
    {
        var operators = await context.Operators
            .Include(o => o.Class)
            .Include(o => o.Subclass)
            .Where(o => o.SubclassId == subclassId)
            .AsNoTracking()
            .ToListAsync();
        return mapper.Map<IEnumerable<OperatorDto>>(operators);
    }
}