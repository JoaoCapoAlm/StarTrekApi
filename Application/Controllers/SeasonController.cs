using Application.Data.ViewModel;
using Application.Services;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class SeasonController : ControllerBase
    {
        private readonly SeasonService _seasonService;

        public SeasonController(SeasonService seasonService)
        {
            _seasonService = seasonService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        /// <response code="200">Seasons list</response>
        [HttpGet]
        public async Task<IEnumerable<SeasonVM>> GetSeasons([FromQuery] byte page = 0, [FromQuery] byte pageSize = 100)
        {
            return await _seasonService.GetSeasons(page, pageSize);
        }

        /// <summary>
        /// Get season by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="404">Not found</response>
        [HttpGet("{id}")]
        public async Task<SeasonVM> GetSeasonById([FromRoute] int id)
        {
            return await _seasonService.GetSeasonById(id);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSeason([FromBody] CreateSeasonWithSerieIdDto dto)
        {
            var season = await _seasonService.CreateSeason(dto);
            return CreatedAtAction(nameof(GetSeasonById), new { id = season.ID }, season);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSeason([FromRoute] byte id, [FromBody] UpdateSeasonDto dto)
        {
            await _seasonService.UpdateSeason(id, dto);
            return NoContent();
        }
    }
}
