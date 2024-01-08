using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly MovieService _movieService;
        public MovieController(MovieService movieService)
        {
            _movieService = movieService;
        }
        [HttpGet]
        public async Task<IActionResult> GetMovies(byte page = 0, byte pageSize = 100)
        {
            var movies = await _movieService.GetMovies(page, pageSize);

            if (movies.Any())
                return Ok(movies);
            else
                return NotFound();
        }

        [HttpGet("{movieId}")]
        public async Task<IActionResult> GetMovie(byte movieId)
        {
            var movie = await _movieService.GetMovie(movieId);

            if (movie == null)
                return NotFound();
            else
                return Ok(movie);
        }
    }
}
