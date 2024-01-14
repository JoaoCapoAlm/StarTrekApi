using Application.Resources;
using Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CastController : ControllerBase
    {
        private readonly CastService _castService;
        public CastController(CastService castService)
        {
            _castService = castService;
        }
        [HttpGet]
        public async Task<IActionResult> GetCastList([FromQuery] byte page = 0, [FromQuery] byte qtd = 100)
        {
            var listCast = await _castService.GetCastList(page, qtd);

            return Ok(listCast);
        }

        [HttpGet("{castId}")]
        public async Task<IActionResult> GetCastById([FromRoute]int castId)
        {
            var cast = await _castService.GetCastById(castId);

            return Ok(cast);
        }
    }
}
