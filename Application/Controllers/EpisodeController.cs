using Application.Data.ViewModel;
using Application.Services;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class EpisodeController : ControllerBase
    {
        private readonly EpisodeService _episodeService;
        public EpisodeController(EpisodeService episodeService)
        {
            _episodeService = episodeService;
        }
        /// <summary>
        /// Get episode list
        /// </summary>
        /// <param name="page">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Episode list</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EpisodeWithSeasonIdVM>>> GetEpisodeList(
            [FromQuery] byte page = 0,
            [FromQuery] byte pageSize = 100
        )
        {
            var list = await _episodeService.GetEpisodeList(page, pageSize);
            return Ok(list);
        }

        /// <summary>
        /// Get episode by ID
        /// </summary>
        /// <param name="id">Episode ID</param>
        /// <returns>Episode</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<EpisodeWithSeasonIdVM>> GetEpisodeById([FromRoute] int id)
        {
            var episode = await _episodeService.GetEpisodeById(id);
            return Ok(episode);
        }

        /// <summary>
        /// Create a new episode
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        /// <response code="201">Created</response>
        /// <response code="400">One or more validation errors occurred</response>
        [HttpPost]
        [ProducesResponseType(201)]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult<EpisodeWithSeasonIdVM>> CreateEpisode([FromBody] CreateEpisodeWithSeasonIdDto dto)
        {
            var episode = await _episodeService.CreateEpisode(dto);
            return CreatedAtAction(nameof(GetEpisodeById), new { id = episode.ID }, episode);
        }

        /// <summary>
        /// Update a episode
        /// </summary>
        /// <param name="id">Episode ID</param>
        /// <param name="dto"></param>
        /// <returns>No content</returns>
        /// <response code="204">Updated</response>
        /// <response code="400">One or more validation errors occurred</response>
        /// <response code="404">Episode not found</response>
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> UpdateEpisode([FromRoute] int id, [FromBody] UpdateEpisodeDto dto)
        {
            await _episodeService.UpdateEpisode(id, dto);

            return NoContent();
        }
    }
}
