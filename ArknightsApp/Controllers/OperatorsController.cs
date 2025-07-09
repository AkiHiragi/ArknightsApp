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
}