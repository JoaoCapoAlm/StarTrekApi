using CrossCutting.AppModel;
using CrossCutting.Enums;
using Domain.DTOs;
using Domain.Interfaces;
using Domain.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [ProducesResponseType(400, Type = typeof(ContentResponse))]
    public class PlaceController : ControllerBase
    {
        private readonly IPlaceService _placeService;
        public PlaceController(IPlaceService placeService)
        {
            _placeService = placeService;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<IEnumerable<PlaceVM>>> GetPlaceList(
            [FromQuery] byte page = 0,
            [FromQuery] byte pageSize = 100,
            [FromQuery] QuadrantEnum? quadrant = null
        )
        {
            var list = await _placeService.GetList(page, pageSize, quadrant);
            return Ok(list);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<PlaceVM>> GetPlaceById([FromRoute] short id)
        {
            var place = await _placeService.GetById(id);
            return Ok(place);
        }

        [HttpPost]
        [ProducesResponseType(201)]
        public async Task<ActionResult<PlaceVM>> CreatePlace([FromBody] CreatePlaceDto dto)
        {
            var place = await _placeService.Create(dto);

            return CreatedAtAction(nameof(GetPlaceById), new { id = place.ID }, place);
        }
    }
}
