using Application.Data;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
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
        public async Task<IActionResult> CreateNewSerieByTmdb([FromRoute] int tmdbId, [FromBody] CreateNewSerieDto dto)
        {
            var serie = await _tmdbService.CreateNewSerieByTmdb(tmdbId, dto);

            return CreatedAtAction(nameof(SerieController.GetSerieById), "Serie", new { id = serie.ID }, serie);
        }
    }
}
