using Domain.DTOs;
using Domain.Interfaces;
using Domain.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class SeasonController : ControllerBase
    {
        private readonly ISeasonService _seasonService;

        public SeasonController(ISeasonService seasonService)
        {
            _seasonService = seasonService;
        }

        /// <summary>
        /// Get seasons list
        /// </summary>
        /// <param name="page" default="0">Page number</param>
        /// <param name="pageSize" default="100">Page size</param>
        /// <returns>Seasons list</returns>
        /// <response code="200">Success - Seasons list</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SeasonWithSerieIdVM>>> GetSeasons([FromQuery] byte page = 0, [FromQuery] byte pageSize = 100)
        {
            var seasonList = await _seasonService.GetList(page, pageSize, null);
            return Ok(seasonList);
        }

        /// <summary>
        /// Get season by ID
        /// </summary>
        /// <param name="id">Season ID</param>
        /// <returns>Season</returns>
        /// <response code="200">Success - Season</response>
        /// <response code="404">Not found</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<SeasonWithSerieIdVM>> GetSeasonById([FromRoute] short id)
        {
            var season = await _seasonService.GetById(id);
            return Ok(season);
        }

        [HttpPost]
        [ProducesResponseType(201)]
        public async Task<ActionResult<SeasonWithSerieIdVM>> CreateSeason([FromBody] CreateSeasonWithSerieIdDto dto)
        {
            var season = await _seasonService.Create(dto);
            return CreatedAtAction(nameof(GetSeasonById), new { id = season.ID }, season);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> UpdateSeason([FromRoute] short id, [FromBody] UpdateSeasonDto dto)
        {
            await _seasonService.Update(id, dto);
            return NoContent();
        }
    }
}
