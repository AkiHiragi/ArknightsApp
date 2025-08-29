using System.Collections.Generic;
using System.Threading.Tasks;
using ArknightsApp.ModelDto;
using ArknightsApp.Models;

namespace ArknightsApp.Services;

public interface IOperatorService
{
    Task<IEnumerable<OperatorDto>> GetAllAsync();
    Task<OperatorDto?> GetByIdAsync(int id);
    Task<OperatorDto> CreateAsync(OperatorDto dto);
    Task<OperatorDto?> UpdateAsync(int id, OperatorDto dto);
    Task<bool> DeleteAsync(int id);
}