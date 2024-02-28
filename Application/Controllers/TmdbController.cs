using Application.Services;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("api/[controller]")]
    [ApiController]
    public class TmdbController : ControllerBase
    {
        private readonly TmdbService _tmdbService;
        public TmdbController(TmdbService tmdbService)
        {
            _tmdbService = tmdbService;
        }
        [HttpPost("serie/{tmdbId}")]
        public async Task<IActionResult> CreateNewSerieByTmdb([FromRoute] int tmdbId, [FromBody] CreateNewSerieByTmdbDto dto)
        {
            var serie = await _tmdbService.CreateNewSerieByTmdb(tmdbId, dto);

            return CreatedAtAction(nameof(SerieController.GetSerieById), nameof(SerieController), new { id = serie.ID }, serie);
        }
    }
}
