using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class CrewController : ControllerBase
    {
        private readonly ICrewService _crewService;
        public CrewController(ICrewService crewService)
        {
            _crewService = crewService;
        }
        [HttpGet]
        public async Task<IActionResult> GetCrewList([FromQuery] byte page = 0, [FromQuery] byte qtd = 100)
        {
            var crewList = await _crewService.GetList(page, qtd);

            return Ok(crewList);
        }

        [HttpGet("{crewId}")]
        public async Task<IActionResult> GetCrewById([FromRoute] int crewId)
        {
            var crew = await _crewService.GetById(crewId);

            return Ok(crew);
        }
    }
}
