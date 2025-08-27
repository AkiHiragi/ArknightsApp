using System.Collections.Generic;
using System.Threading.Tasks;
using ArknightsApp.ModelDto;
using ArknightsApp.Models;

namespace ArknightsApp.Services;

public interface IOperatorService
{
    Task<IEnumerable<Operator>> GetAllAsync();
    Task<Operator?> GetByIdAsync(int id);
    Task<Operator> CreateAsync(OperatorDto dto);
    Task<Operator?> UpdateAsync(int id, OperatorDto dto);
    Task<bool> DeleteAsync(int id);
}