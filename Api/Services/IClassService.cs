using System.Collections.Generic;
using System.Threading.Tasks;
using ArknightsApp.ModelDto;

namespace ArknightsApp.Services;

public interface IClassService
{
    Task<IEnumerable<ClassDto>> GetAllAsync();
    Task<ClassDto?> GetClass(int id);
    Task<IEnumerable<SubclassDto>> GetSubclassesByClassIdAsync(int classId);
    Task<IEnumerable<OperatorDto>> GetOperatorsByClassIdAsync(int classId);
}