using ArknightsApp.Attributes;
using ArknightsApp.DTO;
using ArknightsApp.Models;
using ArknightsApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace ArknightsApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TalentsController : ControllerBase
    {
        private readonly ITalentService _talentService;
        private readonly ILogger<TalentsController> _logger;

        public TalentsController(ITalentService talentService, ILogger<TalentsController> logger)
        {
            _talentService = talentService;
            _logger = logger;
        }

        [HttpGet("operator/{operatorId}")]
        public async Task<ActionResult<List<TalentDto>>> GetOperatorTalents(int operatorId)
        {
            var talents = await _talentService.GetTalentsByOperatorIdAsync(operatorId);
            return Ok(talents);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TalentDto>> GetTalent(int id)
        {
            var talent = await _talentService.GetTalentByIdAsync(id);
            return talent == null ? NotFound() : Ok(talent);
        }

        [HttpPost]
        [RequiredRole(UserRole.Admin)]
        public async Task<ActionResult<TalentDto>> CreateTalent(TalentCreateDto talentDto)
        {
            var createdTalent = await _talentService.CreateTalentAsync(talentDto);
            return CreatedAtAction(nameof(GetTalent), new { id = createdTalent.Id }, createdTalent);
        }

        [HttpPut("{id}")]
        [RequiredRole(UserRole.Admin)]
        public async Task<ActionResult<TalentDto>> UpdateTalent(int id, TalentCreateDto talentDto)
        {
            var updatedTalent = await _talentService.UpdateTalentAsync(id, talentDto);
            return updatedTalent == null ? NotFound() : Ok(updatedTalent);
        }

        [HttpDelete("{id}")]
        [RequiredRole(UserRole.Admin)]
        public async Task<ActionResult> DeleteTalent(int id)
        {
            var result = await _talentService.DeleteTalentAsync(id);
            return result ? NoContent() : NotFound();
        }
    }
}