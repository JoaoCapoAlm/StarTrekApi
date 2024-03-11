using CrossCutting.AppModel;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [ProducesResponseType(400, Type = typeof(ContentResponse))]
    public class SpeciesController : ControllerBase
    {
        private readonly ISpeciesService _speciesService;
        public SpeciesController(ISpeciesService speciesService)
        {
            _speciesService = speciesService;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetSpeciesList([FromQuery] byte page = 0, [FromQuery] byte pageSize = 100)
        {
            var list = await _speciesService.GetList(page, pageSize, null);

            return Ok();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetSpecieById([FromRoute] short id)
        {
            var species = await _speciesService.GetById(id);

            return Ok();
        }
    }
}
