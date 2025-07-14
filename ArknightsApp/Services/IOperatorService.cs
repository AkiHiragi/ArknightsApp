using ArknightsApp.DTO;

namespace ArknightsApp.Services;

public interface IOperatorService
{
    // Основные CRUD операции
    Task<IEnumerable<OperatorDto>> GetAllOperatorsAsync();
    Task<OperatorDetailDto?>       GetOperatorByIdAsync(int              id);
    Task<OperatorDto>              CreateOperatorAsync(OperatorCreateDto operatorDto);
    Task<OperatorDto?>             UpdateOperatorAsync(int               id, OperatorCreateDto operatorDto);
    Task<OperatorEditDto?>         GetOperatorForEditAsync(int           id);
    Task<bool>                     DeleteOperatorAsync(int               id);

    // Фильтрация операторов
    Task<IEnumerable<OperatorDto>> GetOperatorsByRarityAsync(int     rarity);
    Task<IEnumerable<OperatorDto>> GetOperatorsByFactionAsync(string faction);
    Task<IEnumerable<OperatorDto>> GetOperatorsByClassAsync(string   className);

    // Сортировка по датам релиза
    Task<IEnumerable<OperatorDto>> GetOperatorsByReleaseDateAsync(DateTime fromDate, DateTime? toDate = null);
    Task<IEnumerable<OperatorDto>> GetGlobalReleasedOperatorsAsync();
    Task<IEnumerable<OperatorDto>> GetCnOnlyOperatorsAsync();
    Task<IEnumerable<OperatorDto>> GetNewestOperatorsAsync(int count = 10);

    // Методы для расширенного поиска
    Task<IEnumerable<OperatorDto>> SearchOperatorsByNameAsync(string  name);
    Task<PagedResult<OperatorDto>> GetOperatorsPagedAsync(int         page, int pageSize);
    Task<PagedResult<OperatorDto>> SearchOperatorsAsync(SearchRequest request);

    // Расчет характеристик
    Task<OperatorStatsDto> CalculateOperatorStatsAsync(int operatorId, StatsRequestDto request);
}