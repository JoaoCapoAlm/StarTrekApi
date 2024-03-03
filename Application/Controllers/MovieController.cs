using Application.Data.ViewModel;
using Application.Services;
using CrossCutting.Resources;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class MovieController : ControllerBase
    {
        private readonly MovieService _movieService;
        private readonly IStringLocalizer<Messages> _localizer;
        public MovieController(MovieService movieService, IStringLocalizer<Messages> localizer)
        {
            _movieService = movieService;
            _localizer = localizer;
        }

        /// <summary>
        /// Get a movie list
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        /// <response code="200">Movie list</response>
        [HttpGet]
        public async Task<IEnumerable<MovieVM>> GetMovieList([FromQuery] byte page = 0, [FromQuery] byte pageSize = 100)
        {
            return await _movieService.GetMovieList(page, pageSize);
        }

        /// <summary>
        /// Get a movie by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="200">Movie</response>
        [HttpGet("{id}")]
        public async Task<MovieVM> GetMovieById(byte id)
        {
            return await _movieService.GetMovieById(id);
        }

        /// <summary>
        /// Add new movie
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        /// <response code="201">Created</response>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        public async Task<IActionResult> CreateMovie([FromBody] CreateMovieDto dto)
        {
            var movie = await _movieService.CreateMovie(dto);

            return CreatedAtAction(nameof(GetMovieById), new { id = movie.Id }, movie);
        }

        /// <summary>
        /// Update a movie
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        /// <response code="204">Updated / No Content</response>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMovie([FromRoute] short id, [FromBody] UpdateMovieDto dto)
        {
            await _movieService.UpdateMovie(id, dto);
            return NoContent();
        }
    }
}
