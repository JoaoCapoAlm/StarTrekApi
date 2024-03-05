using CrossCutting.Resources;
using Domain;
using Domain.Interfaces;
using Domain.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class MovieController : ControllerBase
    {
        private readonly IMovieService _movieService;
        private readonly IStringLocalizer<Messages> _localizer;
        public MovieController(IMovieService movieService, IStringLocalizer<Messages> localizer)
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
        public async Task<ActionResult<IEnumerable<MovieVM>>> GetMovieList([FromQuery] byte page = 0, [FromQuery] byte pageSize = 100)
        {
            var movieList = await _movieService.GetList(page, pageSize);
            return Ok(movieList);
        }

        /// <summary>
        /// Get a movie by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Movie created</returns>
        /// <response code="200">Movie</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<MovieVM>> GetMovieById(byte id)
        {
            var movie = await _movieService.GetById(id);
            return Ok(movie);
        }

        /// <summary>
        /// Add new movie
        /// </summary>
        /// <param name="dto"></param>
        /// <returns>New movie</returns>
        /// <response code="201">Created</response>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        [ProducesResponseType(201)]
        public async Task<IActionResult> CreateMovie([FromBody] CreateMovieDto dto)
        {
            var movie = await _movieService.Create(dto);

            return CreatedAtAction(nameof(GetMovieById), new { id = movie.Id }, movie);
        }

        /// <summary>
        /// Update a movie
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns>No content</returns>
        /// <response code="204">Updated / No Content</response>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> UpdateMovie([FromRoute] short id, [FromBody] UpdateMovieDto dto)
        {
            await _movieService.Update(id, dto);
            return NoContent();
        }
    }
}
