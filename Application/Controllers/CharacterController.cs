using CrossCutting.AppModel;
using Domain.DTOs;
using Domain.Interfaces;
using Domain.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType<ContentResponse>(400)]
    public class CharacterController : ControllerBase
    {
        private readonly ICharacterService _characterService;
        public CharacterController(ICharacterService characterService)
        {
            _characterService = characterService;
        }
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<IEnumerable<CharacterVM>>> GetCharacterList(
            [FromQuery] byte page = 0, [FromQuery] byte pageSize = 100
        )
        {
            var list = await _characterService.GetList(page, pageSize);
            return Ok(list);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<CharacterVM>> GetCharacterById([FromRoute] int id)
        {
            var character = await _characterService.GetById(id);
            return Ok(character);
        }

        [HttpPost]
        [ProducesResponseType(201)]
        public async Task<ActionResult<CharacterVM>> Create([FromBody] CreateCharacterDto dto)
        {
            var character = await _characterService.Create(dto);

            return CreatedAtAction(nameof(GetCharacterById), new { id = character.ID } , character);
        }
    }
}
