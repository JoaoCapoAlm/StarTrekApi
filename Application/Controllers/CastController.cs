using Application.Services;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> Get([FromQuery] byte? page, [FromQuery] byte? qtd)
        {
            var listCast = await _castService.GetCasts(page ?? 0, qtd ?? 100);

            return Ok(new { message = listCast });
        }
    }
}
