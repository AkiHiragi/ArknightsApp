using System.Collections.Generic;
using System.Threading.Tasks;
using ArknightsApp.ModelDto;

namespace ArknightsApp.Services;

public interface ISubclassService
{
    Task<IEnumerable<SubclassDto>> GetAllAsync();
    Task<SubclassDto?> GetByIdAsync(int id);
    Task<SubclassDto> CreateAsync(CreateSubclassDto dto);
    Task<SubclassDto?> UpdateAsync(int id, CreateSubclassDto dto);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<OperatorDto>> GetOperatorsBySubclassIdAsync(int subclassId);
}