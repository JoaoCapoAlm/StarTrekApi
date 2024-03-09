using Domain.Interfaces;
using Domain.ViewModel;
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
        public async Task<ActionResult<IEnumerable<CrewVM>>> GetCrewList([FromQuery] byte page = 0, [FromQuery] byte pageSize = 100)
        {
            var crewList = await _crewService.GetList(page, pageSize, x => true);

            return Ok(crewList);
        }

        [HttpGet("{crewId}")]
        public async Task<ActionResult<CrewVM>> GetCrewById([FromRoute] int crewId)
        {
            var crew = await _crewService.GetById(crewId);

            return Ok(crew);
        }
    }
}
