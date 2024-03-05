using Domain;
using Domain.Interfaces;
using Domain.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class SerieController : ControllerBase
    {
        private readonly ISerieService _serieService;

        public SerieController(ISerieService serieService)
        {
            _serieService = serieService;
        }

        /// <summary>
        /// List of Star Trek series
        /// </summary>
        /// <param name="page">Page number</param>
        /// <param name="pageSize">Number of series on the page. The value must be between 0 and 100.</param>
        /// <returns>List itens</returns>
        /// <response code="400">Agument invalid</response>
        /// <response code="404">Not found</response>
        /// <response code="500">Internal error</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SerieVM>>> GetList(
            [FromQuery] byte page = 0,
            [FromQuery] byte pageSize = 100
        )
        {
            var series = await _serieService.GetList(page, pageSize);
            return Ok(series);
        }

        /// <summary>
        /// Star Trek series with ID
        /// </summary>
        /// <param name="id">Serie's ID</param>
        /// <returns>Return serie</returns>
        /// <response code="400">ID invalid</response>
        /// <response code="404">Not found</response>
        /// <response code="500">Internal error</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<SerieVM>> GetById([FromRoute] short id)
        {
            var serie = await _serieService.GetById(id);
            return Ok(serie);
        }

        /// <summary>
        /// Create new serie
        /// </summary>
        /// <param name="dto">Informtion about serie</param>
        /// <returns>Return new serie</returns>
        /// <response code="201">Created</response>
        /// <response code="400">Invalid data</response>
        /// <response code="500">Internal error</response>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        [ProducesResponseType(201)]
        public async Task<ActionResult<SerieVM>> CreateNewSerie([FromBody] CreateSerieDto dto)
        {
            var newSerie = await _serieService.Create(dto);
            return CreatedAtAction(nameof(GetById), new { id = newSerie.ID }, newSerie);
        }

        /// <summary>
        /// Update serie data
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <response code="204">Updated / No Content</response>
        /// <response code="400">Invalid data</response>
        /// <response code="404">Not found</response>
        /// <response code="500">Internal error</response>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> UpdateSerieById([FromRoute] short id, [FromBody] UpdateSerieDto dto)
        {
            await _serieService.Update(id, dto);
            return NoContent();
        }
    }
}
