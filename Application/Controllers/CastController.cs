using Application.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CastController : ControllerBase
    {
        private readonly IStringLocalizer<Messages> _localizer;
        public CastController(IStringLocalizer<Messages> localizer)
        {
            _localizer = localizer;
        }
        [HttpGet]
        public IActionResult Get([FromQuery] byte page = 0, [FromQuery] byte qtd = 100)
        {
            return Ok(new { message = _localizer["HelloWord"].Value });
        }
    }
}
