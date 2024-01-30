using Application.Configurations;
using Application.Data;
using Application.Resources;
using Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly MovieService _movieService;
        private readonly IStringLocalizer<Messages> _localizer;
        public MovieController(MovieService movieService, IStringLocalizer<Messages> localizer)
        {
            _movieService = movieService;
            _localizer = localizer;
        }
        [HttpGet]
        public async Task<IActionResult> GetMovieList(byte page = 0, byte pageSize = 100)
        {
            var movies = await _movieService.GetMovieList(page, pageSize);

            return Ok(movies);
        }

        [HttpGet("{movieId}")]
        public async Task<IActionResult> GetMovie(byte movieId)
        {
            var movie = await _movieService.GetMovieById(movieId);

            return Ok(movie);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMovie(CreateMovieDto dto)
        {
            var movie = await _movieService.CreateMovie(dto);

            return CreatedAtAction(nameof(GetMovie), nameof(SerieController), new { id = movie.Id }, movie);
        }
        [HttpPut("{movieId}")]
        public async Task<IActionResult> UpdateMovie([FromRoute] byte movieId, [FromBody] UpdateMovieDto dto)
        {
            await _movieService.UpdateMovie(movieId, dto);
            return NoContent();
        }
    }
}
