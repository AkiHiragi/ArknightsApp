using ArknightsApp.Models;

namespace ArknightsApp.Data;

public interface IOperatorRepository
{
    Task<IEnumerable<Operator>> GetAllAsync();
    Task<Operator?>             GetByIdAsync(int         id);
    Task<Operator>              CreateAsync(Operator     op);
    Task<Operator?>             UpdateAsync(Operator     op);
    Task<bool>                  DeleteAsync(int          id);
    Task<IEnumerable<Operator>> GetByRarityAsync(int     rarity);
    Task<IEnumerable<Operator>> GetByFactionAsync(string faction);
    Task<IEnumerable<Operator>> GetByClassAsync(string   className);
    Task<IEnumerable<Operator>> GetNewestAsync(int       count);
    Task<IEnumerable<Operator>> GetGlobalReleasedAsync();
    Task<IEnumerable<Operator>> GetCnOnlyAsync();
    Task<IEnumerable<Operator>> GetByReleaseDateAsync(DateTime fromDate, DateTime? toDate);
}