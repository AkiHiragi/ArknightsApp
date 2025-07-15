using ArknightsApp.Attributes;
using ArknightsApp.DTO;
using ArknightsApp.Models;
using ArknightsApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace ArknightsApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SkillController : ControllerBase
{
    private readonly ISkillService            _skillService;
    private readonly ILogger<SkillController> _logger;

    public SkillController(ISkillService skillService, ILogger<SkillController> logger)
    {
        _skillService = skillService;
        _logger       = logger;
    }

    [HttpGet("operator/{operatorId}")]
    public async Task<ActionResult<List<SkillDto>>> GetOperatorSkills(int operatorId)
    {
        var skills = await _skillService.GetSkillsByOperatorIdAsync(operatorId);
        return Ok(skills);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SkillDto>> GetSkill(int id)
    {
        var skill = await _skillService.GetSkillByIdAsync(id);
        return skill == null ? NotFound() : Ok(skill);
    }

    [HttpPost]
    [RequiredRole(UserRole.Admin)]
    public async Task<ActionResult<SkillDto>> CreateSkill(SkillCreateDto skillDto)
    {
        var createdSkill = await _skillService.CreateSkillAsync(skillDto);
        return CreatedAtAction(nameof(GetSkill), new { id = createdSkill.Id }, createdSkill);
    }

    [HttpPut("{id}")]
    [RequiredRole(UserRole.Admin)]
    public async Task<ActionResult<SkillDto>> UpdateSkill(int id, SkillCreateDto skillDto)
    {
        var updatedSkill = await _skillService.UpdateSkillAsync(id, skillDto);
        return updatedSkill == null ? NotFound() : Ok(updatedSkill);
    }
    
    [HttpDelete("{id}")]
    [RequiredRole(UserRole.Admin)]
    public async Task<IActionResult> DeleteSkill(int id)
    {
        var result = await _skillService.DeleteSkillAsync(id);
        return result ? NoContent() : NotFound();
    }
}