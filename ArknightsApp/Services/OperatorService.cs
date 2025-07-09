using ArknightsApp.Data;
using ArknightsApp.DTO;
using ArknightsApp.Models;
using ArknightsApp.Validators;
using AutoMapper;
using FluentValidation;

namespace ArknightsApp.Services;

public class OperatorService : IOperatorService
{
    private readonly IMapper                       _mapper;
    private readonly ILogger<OperatorService>      _logger;
    private readonly IValidator<OperatorCreateDto> _createValidator;
    private readonly IOperatorRepository           _repository;

    public OperatorService(
        IMapper                       mapper,
        ILogger<OperatorService>      logger,
        IValidator<OperatorCreateDto> createValidator,
        IOperatorRepository           repository)
    {
        _mapper          = mapper;
        _logger          = logger;
        _createValidator = createValidator;
        _repository      = repository;
    }

    public async Task<IEnumerable<OperatorDto>> GetAllOperatorsAsync()
    {
        _logger.LogInformation("Получение всех операторов");
        var operators = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<OperatorDto>>(operators);
    }

    public async Task<OperatorDetailDto?> GetOperatorByIdAsync(int id)
    {
        _logger.LogInformation("Получение оператора с ID: {OperatorId}", id);
        var op = await _repository.GetByIdAsync(id);
        return op != null ? _mapper.Map<OperatorDetailDto>(op) : null;
    }

    public async Task<OperatorDto> CreateOperatorAsync(OperatorCreateDto operatorDto)
    {
        _logger.LogInformation("Создание нового оператора: {OperatorName}", operatorDto.Name);

        // Валидация DTO
        var validationResult = await _createValidator.ValidateAsync(operatorDto);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        // Создаем оператора
        var newOperator     = _mapper.Map<Operator>(operatorDto);
        var createdOperator = await _repository.CreateAsync(newOperator);

        return _mapper.Map<OperatorDto>(createdOperator);
    }

    public async Task<OperatorDto?> UpdateOperatorAsync(int id, OperatorCreateDto operatorDto)
    {
        _logger.LogInformation("Обновление оператора с ID: {OperatorId}", id);

        var validationResult = await _createValidator.ValidateAsync(operatorDto);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var existingOperator = await _repository.GetByIdAsync(id);
        if (existingOperator == null)
        {
            return null;
        }

        // Обновляем поля
        _mapper.Map(operatorDto, existingOperator);
        var updatedOperator = await _repository.UpdateAsync(existingOperator);

        return updatedOperator != null ? _mapper.Map<OperatorDto>(updatedOperator) : null;
    }

    public async Task<bool> DeleteOperatorAsync(int id)
    {
        _logger.LogInformation("Удаление оператора с ID: {OperatorId}", id);
        return await _repository.DeleteAsync(id);
    }


    public async Task<OperatorStatsDto> CalculateOperatorStatsAsync(int operatorId, StatsRequestDto request)
    {
        _logger.LogInformation("Расчет характеристик для оператора {OperatorId}", operatorId);

        var op = await _repository.GetByIdAsync(operatorId);
        if (op?.GrowthTemplate == null)
        {
            throw new ArgumentException($"Оператор с ID {operatorId} не найден или не имеет шаблона характеристик");
        }

        // Валидация с учетом редкости
        var validator        = new StatRequestWithRarityValidator(op.Rarity);
        var validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        // Расчет характеристик
        var calculatedStats = OperatorStatsCalculator.CalculateStats(
            op.GrowthTemplate, request.Level, request.Elite, request.TrustLevel);

        return _mapper.Map<OperatorStatsDto>(calculatedStats);
    }

    public async Task<IEnumerable<OperatorDto>> GetOperatorsByRarityAsync(int rarity)
    {
        var operators = await _repository.GetByRarityAsync(rarity);
        return _mapper.Map<IEnumerable<OperatorDto>>(operators);
    }

    public async Task<IEnumerable<OperatorDto>> GetOperatorsByFactionAsync(string faction)
    {
        var operators = await _repository.GetByFactionAsync(faction);
        return _mapper.Map<IEnumerable<OperatorDto>>(operators);
    }

    public async Task<IEnumerable<OperatorDto>> GetOperatorsByClassAsync(string className)
    {
        var operators = await _repository.GetByClassAsync(className);
        return _mapper.Map<IEnumerable<OperatorDto>>(operators);
    }

    public async Task<IEnumerable<OperatorDto>> GetGlobalReleasedOperatorsAsync()
    {
        var operators = await _repository.GetGlobalReleasedAsync();
        return _mapper.Map<IEnumerable<OperatorDto>>(operators);
    }

    public async Task<IEnumerable<OperatorDto>> GetCnOnlyOperatorsAsync()
    {
        var operators = await _repository.GetCnOnlyAsync();
        return _mapper.Map<IEnumerable<OperatorDto>>(operators);
    }


    public async Task<IEnumerable<OperatorDto>> GetNewestOperatorsAsync(int count = 10)
    {
        var operators = await _repository.GetNewestAsync(count);
        return _mapper.Map<IEnumerable<OperatorDto>>(operators);
    }


    public async Task<IEnumerable<OperatorDto>> SearchOperatorsByNameAsync(string name)
    {
        _logger.LogInformation("Поиск операторов по имени: {Name}", name);
        var operators = await _repository.SearchByNameAsync(name);
        return _mapper.Map<IEnumerable<OperatorDto>>(operators);
    }

    public async Task<PagedResult<OperatorDto>> GetOperatorsPagedAsync(int page, int pageSize)
    {
        _logger.LogInformation("Получение операторов с пагинацией: страница {Page}, размер {PageSize}",
                               page, pageSize);
        var pagedResult = await _repository.GetPagedAsync(page, pageSize);

        return new PagedResult<OperatorDto>
        {
            Items      = _mapper.Map<List<OperatorDto>>(pagedResult.Items),
            TotalCount = pagedResult.TotalCount,
            Page       = pagedResult.Page,
            PageSize   = pagedResult.PageSize
        };
    }

    public async Task<PagedResult<OperatorDto>> SearchOperatorsAsync(SearchRequest request)
    {
        _logger.LogInformation("Расширенный поиск операторов: {Request}",
                               System.Text.Json.JsonSerializer.Serialize(request));

        var pagedResult = await _repository.SearchAsync(request);

        return new PagedResult<OperatorDto>
        {
            Items      = _mapper.Map<List<OperatorDto>>(pagedResult.Items),
            TotalCount = pagedResult.TotalCount,
            Page       = pagedResult.Page,
            PageSize   = pagedResult.PageSize
        };
    }

    public async Task<IEnumerable<OperatorDto>> GetOperatorsByReleaseDateAsync(
        DateTime fromDate, DateTime? toDate = null)
    {
        var operators = await _repository.GetByReleaseDateAsync(fromDate, toDate);
        return _mapper.Map<IEnumerable<OperatorDto>>(operators);
    }
}