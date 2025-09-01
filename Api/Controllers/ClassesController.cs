using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ArknightsApp.ModelDto;
using ArknightsApp.Services;

namespace ArknightsApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassesController(IClassService service) : ControllerBase
    {
        // GET: api/Classes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClassDto>>> GetClasses()
        {
            return Ok(await service.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClassDto>> GetClass(int id)
        {
            var classDto = await service.GetClass(id);
            return classDto == null ? NotFound() : Ok(classDto);
        }

        [HttpGet("{id}/subclasses")]
        public async Task<ActionResult<IEnumerable<SubclassDto>>> GetSubclasses(int id)
        {
            return Ok(await service.GetSubclassesByClassIdAsync(id));
        }

        [HttpGet("{id}/operators")]
        public async Task<ActionResult<IEnumerable<OperatorDto>>> GetOperators(int id)
        {
            return Ok(await service.GetOperatorsByClassIdAsync(id));
        }
    }
}