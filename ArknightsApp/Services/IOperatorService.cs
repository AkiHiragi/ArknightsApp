using ArknightsApp.DTO;

namespace ArknightsApp.Services;

public interface IOperatorService
{
    Task<IEnumerable<OperatorDto>> GetAllOperatorsAsync();
    Task<OperatorDetailDto?>       GetOperatorByIdAsync(int              id);
    Task<OperatorDto>              CreateOperatorAsync(OperatorCreateDto operatorDto);
    Task<OperatorDto?>             UpdateOperatorAsync(int               id, OperatorCreateDto operatorDto);
    Task<bool>                     DeleteOperatorAsync(int               id);

    Task<IEnumerable<OperatorDto>> GetOperatorsByRarityAsync(int     rarity);
    Task<IEnumerable<OperatorDto>> GetOperatorsByFactionAsync(string faction);
    Task<IEnumerable<OperatorDto>> GetOperatorsByClassAsync(string   className);

    Task<IEnumerable<OperatorDto>> GetOperatorsByReleaseDateAsync(DateTime fromDate, DateTime? toDate = null);
    Task<IEnumerable<OperatorDto>> GetGlobalReleasedOperatorsAsync();
    Task<IEnumerable<OperatorDto>> GetCnOnlyOperatorsAsync();
    Task<IEnumerable<OperatorDto>> GetNewestOperatorsAsync(int count = 10);

    Task<OperatorStatsDto> CalculateOperatorStatsAsync(int operatorId, StatsRequestDto request);
}