using Application.Services;
using CrossCutting.Resources;
using Domain;
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
        public async Task<IActionResult> GetMovieList([FromQuery]byte page = 0, [FromQuery]byte pageSize = 100)
        {
            var movies = await _movieService.GetMovieList(page, pageSize);

            return Ok(movies);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMovieById(byte id)
        {
            var movie = await _movieService.GetMovieById(id);

            return Ok(movie);
        }

        /// <summary>
        /// Criação de filme
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateMovie([FromBody]CreateMovieDto dto)
        {
            var movie = await _movieService.CreateMovie(dto);

            return CreatedAtAction(nameof(GetMovieById), new { id = movie.Id}, movie);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMovie([FromRoute] short id, [FromBody] UpdateMovieDto dto)
        {
            await _movieService.UpdateMovie(id, dto);
            return NoContent();
        }
    }
}
