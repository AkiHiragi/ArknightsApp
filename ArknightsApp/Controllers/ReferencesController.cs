using Microsoft.AspNetCore.Mvc;
using ArknightsApp.Services;
using ArknightsApp.DTO;

namespace ArknightsApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReferencesController : ControllerBase
    {
        private readonly IReferenceService _referenceService;

        public ReferencesController(IReferenceService referenceService)
        {
            _referenceService = referenceService;
        }

        [HttpGet("classes")]
        public async Task<ActionResult<List<OperatorClassDto>>> GetOperatorClasses()
        {
            var classes = await _referenceService.GetOperatorClassesAsync();
            return Ok(classes);
        }

        [HttpGet("classes/{classId}/subclasses")]
        public async Task<ActionResult<List<SubClassDto>>> GetSubClasses(int classId)
        {
            var subClasses = await _referenceService.GetSubClassesByClassIdAsync(classId);
            return Ok(subClasses);
        }

        [HttpGet("factions")]
        public async Task<ActionResult<List<FactionDto>>> GetFactions()
        {
            var factions = await _referenceService.GetFactionsAsync();
            return Ok(factions);
        }

        [HttpGet("create-form-data")]
        public async Task<ActionResult<OperatorCreateFormDto>> GetCreateFormData()
        {
            var formData = await _referenceService.GetOperatorCreateFormDataAsync();
            return Ok(formData);
        }
    }
}