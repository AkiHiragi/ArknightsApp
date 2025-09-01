using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArknightsApp.Data;
using ArknightsApp.ModelDto;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ArknightsApp.Services;

public class ClassService(ApplicationDbContext context, IMapper mapper) : IClassService
{
    public async Task<IEnumerable<ClassDto>> GetAllAsync()
    {
        var classes = await context.Classes.AsNoTracking().ToArrayAsync();
        return mapper.Map<IEnumerable<ClassDto>>(classes);
    }

    public async Task<ClassDto?> GetClass(int id)
    {
        var classEntity = await context.Classes.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        return classEntity == null ? null : mapper.Map<ClassDto>(classEntity);
    }

    public async Task<IEnumerable<SubclassDto>> GetSubclassesByClassIdAsync(int classId)
    {
        var subClasses = await context.Subclasses
            .Include(s => s.Class)
            .Where(s => s.ClassId == classId)
            .AsNoTracking()
            .ToListAsync();

        return mapper.Map<IEnumerable<SubclassDto>>(subClasses);
    }

    public async Task<IEnumerable<OperatorDto>> GetOperatorsByClassIdAsync(int classId)
    {
        var operators = await context.Operators
            .Include(o => o.Class)
            .Include(o => o.Subclass)
            .Where(o => o.ClassId == classId)
            .AsNoTracking()
            .ToListAsync();
        
        return mapper.Map<IEnumerable<OperatorDto>>(operators);
    }
}