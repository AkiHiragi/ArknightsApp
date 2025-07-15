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
    private readonly ISkillTemplateService    _skillTemplateService;

    public SkillController(ISkillService         skillService, ILogger<SkillController> logger,
                           ISkillTemplateService skillTemplateService)
    {
        _skillService  = skillService;
        _logger        = logger;
        _skillTemplateService = skillTemplateService;
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

    [HttpPost("create-with-template")]
    [RequiredRole(UserRole.Admin)]
    public async Task<ActionResult<SkillDto>> CreateSkillWithTemplate(SkillCreateWithTemplateDto dto)
    {
        try
        {
            var levels = _skillTemplateService.GenerateSkillLevels(dto.Template);

            var skillDto = new SkillCreateDto
            {
                Name            = dto.Name,
                BaseDescription = dto.Template.BaseDescription,
                IconUrl         = dto.IconUrl,
                OperatorId      = dto.OperatorId,
                Levels          = levels
            };

            var createdSkill = await _skillService.CreateSkillAsync(skillDto);
            return CreatedAtAction(nameof(GetSkill), new { id = createdSkill.Id }, createdSkill);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка создания скилла с шаблоном");
            return BadRequest(new
            {
                message = "Ошибка создания скилла",
                error = ex.Message,
                innerError = ex.InnerException?.Message
            });
        }
    }
}