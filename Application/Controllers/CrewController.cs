using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CrewController : ControllerBase
    {
        private readonly CrewService _crewService;
        public CrewController(CrewService crewService)
        {
            _crewService = crewService;
        }
        [HttpGet]
        public async Task<IActionResult> GetCrewList([FromQuery] byte page = 0, [FromQuery] byte qtd = 100)
        {
            var crewList = await _crewService.GetCrewList(page, qtd);

            return Ok(crewList);
        }

        [HttpGet("{crewId}")]
        public async Task<IActionResult> GetCrewById([FromRoute] int crewId)
        {
            var crew = await _crewService.GetCrewById(crewId);

            return Ok(crew);
        }
    }
}
