using ArknightsApp.DTO;
using ArknightsApp.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace ArknightsApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OperatorsController : ControllerBase
{
    private readonly IOperatorService             _operatorService;
    private readonly ILogger<OperatorsController> _logger;


    public OperatorsController(IOperatorService operatorService, ILogger<OperatorsController> logger)
    {
        _operatorService = operatorService;
        _logger          = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<OperatorDto>>> GetAllOperators()
    {
        var operators = await _operatorService.GetAllOperatorsAsync();
        return Ok(operators);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OperatorDetailDto>> GetOperator(int id)
    {
        var op = await _operatorService.GetOperatorByIdAsync(id);
        return op == null ? NotFound() : Ok(op);
    }

    [HttpPost]
    public async Task<ActionResult<OperatorDto>> CreateOperator(OperatorCreateDto operatorDto)
    {
        try
        {
            var createdOperator = await _operatorService.CreateOperatorAsync(operatorDto);
            return CreatedAtAction(nameof(GetOperator), new { id = createdOperator.Id }, createdOperator);
        }
        catch (ValidationException ex)
        {
            return BadRequest(ex.Errors.Select(e => new { Field = e.PropertyName, Error = e.ErrorMessage }));
        }
    }

    [HttpGet("rarity/{rarity}")]
    public async Task<ActionResult<IEnumerable<OperatorDto>>> GetOperatorsByRarity(int rarity)
    {
        var operators = await _operatorService.GetOperatorsByRarityAsync(rarity);
        return Ok(operators);
    }

    [HttpGet("newest")]
    public async Task<ActionResult<IEnumerable<OperatorDto>>> GetNewestOperators([FromQuery] int count = 10)
    {
        var operators = await _operatorService.GetNewestOperatorsAsync(count);
        return Ok(operators);
    }

    [HttpPost("{id}/stats")]
    public async Task<ActionResult<OperatorStatsDto>> CalculateStats(int id, StatsRequestDto request)
    {
        try
        {
            var stats = await _operatorService.CalculateOperatorStatsAsync(id, request);
            return Ok(stats);
        }
        catch (ValidationException ex)
        {
            return BadRequest(ex.Errors.Select(e => new { Field = e.PropertyName, Error = e.ErrorMessage }));
        }
        catch (ArgumentException ex)
        {
            return NotFound(new { Error = ex.Message });
        }
    }
    
    [HttpPut("{id}")]
    public async Task<ActionResult<OperatorDto>> UpdateOperator(int id, OperatorCreateDto operatorDto)
    {
        try
        {
            var updatedOperator = await _operatorService.UpdateOperatorAsync(id, operatorDto);
            return updatedOperator == null ? NotFound() : Ok(updatedOperator);
        }
        catch (ValidationException ex)
        {
            return BadRequest(ex.Errors.Select(e => new { Field = e.PropertyName, Error = e.ErrorMessage }));
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteOperator(int id)
    {
        var result = await _operatorService.DeleteOperatorAsync(id);
        return result ? NoContent() : NotFound();
    }

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<OperatorDto>>> SearchOperators([FromQuery] string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return BadRequest("Параметр поиска 'name' не может быть пустым");
        }

        var operators = await _operatorService.SearchOperatorsByNameAsync(name);
        return Ok(operators);
    }
    
    [HttpGet("paged")]
    public async Task<ActionResult<PagedResult<OperatorDto>>> GetOperatorsPaged(
        [FromQuery] int page     = 1, 
        [FromQuery] int pageSize = 10)
    {
        if (page < 1) page                           = 1;
        if (pageSize < 1 || pageSize > 100) pageSize = 10;

        var result = await _operatorService.GetOperatorsPagedAsync(page, pageSize);
        return Ok(result);
    }
    
    [HttpPost("search/advanced")]
    public async Task<ActionResult<PagedResult<OperatorDto>>> SearchOperatorsAdvanced(SearchRequest request)
    {
        var result = await _operatorService.SearchOperatorsAsync(request);
        return Ok(result);
    }
    
    [HttpGet("search/advanced")]
    public async Task<ActionResult<PagedResult<OperatorDto>>> SearchOperatorsAdvancedGet(
        [FromQuery] string? name           = null,
        [FromQuery] int?    rarity         = null,
        [FromQuery] string? className      = null,
        [FromQuery] string? factionName    = null,
        [FromQuery] int     page           = 1,
        [FromQuery] int     pageSize       = 10,
        [FromQuery] string  sortBy         = "Name",
        [FromQuery] bool    sortDescending = false)
    {
        var request = new SearchRequest
        {
            Name           = name,
            Rarity         = rarity,
            ClassName      = className,
            FactionName    = factionName,
            Page           = page,
            PageSize       = pageSize,
            SortBy         = sortBy,
            SortDescending = sortDescending
        };

        var result = await _operatorService.SearchOperatorsAsync(request);
        return Ok(result);
    }
}