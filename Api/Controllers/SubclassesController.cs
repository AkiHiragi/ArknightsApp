using System.Collections.Generic;
using System.Threading.Tasks;
using ArknightsApp.ModelDto;
using ArknightsApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace ArknightsApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SubclassesController(ISubclassService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SubclassDto>>> GetSubclasses()
    {
        return Ok(await service.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SubclassDto>> GetSubclass(int id)
    {
        var subclass = await service.GetByIdAsync(id);
        return subclass == null ? NotFound() : Ok(subclass);
    }

    [HttpPost]
    public async Task<ActionResult<SubclassDto>> PostSubclass([FromBody] CreateSubclassDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var subclass = await service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetSubclass), new { id = subclass.Id }, subclass);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutSubclass(int id, [FromBody] CreateSubclassDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var subclass = await service.UpdateAsync(id, dto);
        return subclass != null ? Ok(subclass) : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSubclass(int id)
    {
        return await service.DeleteAsync(id) ? NoContent() : NotFound();
    }

    [HttpGet("{id}/operators")]
    public async Task<ActionResult<IEnumerable<OperatorDto>>> GetOperators(int id)
    {
        return Ok(await service.GetOperatorsBySubclassIdAsync(id));
    }
}