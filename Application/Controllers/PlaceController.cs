using CrossCutting.Enums;
using Domain.Interfaces;
using Domain.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class PlaceController : ControllerBase
    {
        private readonly IPlaceService _placeService;
        public PlaceController(IPlaceService placeService)
        {
            _placeService = placeService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlaceVM>>> GetPlaceList(
            [FromQuery] byte page = 0,
            [FromQuery] byte pageSize = 100,
            [FromQuery] QuadrantEnum? quadrant = null
        )
        {
            var list = await _placeService.GetList(
                page,
                pageSize,
                x => !quadrant.HasValue || x.QuadrantId == quadrant.Value.GetHashCode());
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PlaceVM>> GetPlaceById([FromRoute] short id)
        {
            var place = await _placeService.GetById(id);
            return Ok(place);
        }
    }
}
