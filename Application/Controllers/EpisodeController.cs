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
        public async Task<IEnumerable<EpisodeWithSeasonIdVM>> GetEpisodeList(
            [FromQuery] byte page = 0,
            [FromQuery] byte pageSize = 100
        )
        {
            return await _episodeService.GetEpisodeList(page, pageSize);
        }

        [HttpGet("{id}")]
        public async Task<EpisodeWithSeasonIdVM> GetEpisodeById([FromRoute] int id)
        {
            return await _episodeService.GetEpisodeById(id);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEpisode([FromBody] CreateEpisodeWithSeasonIdDto dto)
        {
            var ep = await _episodeService.CreateEpisode(dto);

            return CreatedAtAction(nameof(GetEpisodeById), new { id = ep.ID }, ep);
        }
    }
}
