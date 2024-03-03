﻿using Application.Data.ViewModel;
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
        /// Get seasons list
        /// </summary>
        /// <param name="page" default="0">Page number</param>
        /// <param name="pageSize" default="100">Page size</param>
        /// <returns>Seasons list</returns>
        /// <response code="200">Success - Seasons list</response>
        [HttpGet]
        public async Task<IEnumerable<SeasonWithSerieIdVM>> GetSeasons([FromQuery] byte page = 0, [FromQuery] byte pageSize = 100)
        {
            return await _seasonService.GetSeasons(page, pageSize);
        }

        /// <summary>
        /// Get season by ID
        /// </summary>
        /// <param name="id">Season ID</param>
        /// <returns>Season</returns>
        /// <response code="200">Success - Season</response>
        /// <response code="404">Not found</response>
        [HttpGet("{id}")]
        public async Task<SeasonWithSerieIdVM> GetSeasonById([FromRoute] int id)
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
