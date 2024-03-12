using CrossCutting.AppModel;
using Domain.DTOs;
using Domain.Interfaces;
using Domain.ViewModel;
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
        public async Task<ActionResult<IEnumerable<SpeciesVM>>> GetSpeciesList([FromQuery] byte page = 0, [FromQuery] byte pageSize = 100)
        {
            var list = await _speciesService.GetList(page, pageSize, null);

            return Ok(list);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<SpeciesVM>> GetSpeciesById([FromRoute] short id)
        {
            var species = await _speciesService.GetById(id);

            return Ok(species);
        }

        [HttpPost]
        [ProducesResponseType(201)]
        public async Task<ActionResult<SpeciesVM>> CreateSpecies(CreateSpeciesDto dto)
        {
            var species = await _speciesService.CreateSpecies(dto);

            return CreatedAtAction(nameof(GetSpeciesById), new { id = species.ID }, species);
        }
    }
}
