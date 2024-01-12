using Application.Configurations;
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
        private readonly IStringLocalizer<Messages> _localizer;
        public CastController(CastService castService, IStringLocalizer<Messages> localizer)
        {
            _castService = castService;
            _localizer = localizer;
        }
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] byte page = 0, [FromQuery] byte qtd = 100)
        {
            var listCast = await _castService.GetCasts(page, qtd);

            if (listCast.Any())
                return Ok(listCast);

            throw new ExceptionNofFound(_localizer["NotFound"].Value);
        }

        [HttpGet("{castId}")]
        public async Task<IActionResult> GetCast([FromRoute]byte castId)
        {
            var cast = await _castService.GetCast(castId);
            if (cast == null)
                throw new ExceptionNofFound(_localizer["NotFound"].Value);

            return Ok(cast);
        }
    }
}
