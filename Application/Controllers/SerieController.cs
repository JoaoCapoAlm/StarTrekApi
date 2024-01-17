﻿using Application.Data.ViewModel;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class SerieController : ControllerBase
    {
        private readonly SerieService _serieService;

        public SerieController(SerieService serieService)
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
        public async Task<IEnumerable<SerieVM>> GetSeriesList([FromQuery] byte page = 0, [FromQuery] byte pageSize = 100)
        {
            return await _serieService.GetSeriesList(page, pageSize);
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
        public async Task<SerieVM> GetSerieById([FromRoute] byte id)
        {
            return await _serieService.GetSerieById(id);
        }
    }
}