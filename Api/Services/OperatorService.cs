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
    public async Task<IEnumerable<Operator>> GetAllAsync()
        => await context.Operators.AsNoTracking().ToListAsync();

    public async Task<Operator?> GetByIdAsync(int id)
        => await context.Operators.FindAsync(id);

    public async Task<Operator> CreateAsync(OperatorDto dto)
    {
        var op = mapper.Map<Operator>(dto);

        context.Operators.Add(op);
        await context.SaveChangesAsync();

        return op;
    }

    public async Task<Operator?> UpdateAsync(int id, OperatorDto dto)
    {
        var op = await GetByIdAsync(id);
        if (op == null) return null;

        mapper.Map(dto, op);
        
        await context.SaveChangesAsync();
        return op;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var op = await GetByIdAsync(id);
        if (op == null) return false;
        context.Operators.Remove(op);
        await context.SaveChangesAsync();
        return true;
    }
}