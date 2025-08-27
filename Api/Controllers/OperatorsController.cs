using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ArknightsApp.ModelDto;
using ArknightsApp.Models;
using ArknightsApp.Services;

namespace ArknightsApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OperatorsController(IOperatorService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Operator>>> GetOperators()
    {
        return Ok(await service.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Operator>> GetOperator(int id)
    {
        var op = await service.GetByIdAsync(id);
        return op == null ? NotFound() : Ok(op);
    }

    [HttpPost]
    public async Task<ActionResult<Operator>> PostOperator([FromBody] OperatorDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var op = await service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetOperator), new { id = op.Id }, op);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutOperator(int id, [FromBody] OperatorDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var op = await service.UpdateAsync(id, dto);
        return op != null ? Ok(op) : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOperator(int id)
    {
        return await service.DeleteAsync(id) ? NoContent() : NotFound();
    }
}