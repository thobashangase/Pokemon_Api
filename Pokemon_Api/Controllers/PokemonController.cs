using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pokemon_Api.Models;
using Pokemon_Api.Services;

namespace Pokemon_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : ControllerBase
    {
        private readonly ILogger<PokemonController> _logger;
        private readonly IPokemonService _pokemonService;

        public PokemonController(ILogger<PokemonController> logger, IPokemonService pokemonApiClient)
        {
            _logger = logger;
            _pokemonService = pokemonApiClient;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Pokemon>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAsync()
        {
            try
            {
                var pokemons = await _pokemonService.GetPokemonsAsync(cancellationToken: CancellationToken.None);

                return Ok(pokemons);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpGet("filtered/{term}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Pokemon>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetFilteredAsync(string term)
        {
            if (string.IsNullOrWhiteSpace(term))
            {
                return BadRequest(new { message = "Please provide a search term!" });
            }

            try
            {
                var pokemons = await _pokemonService.GetPokemonsFilteredAsync(term, cancellationToken: CancellationToken.None);

                return Ok(pokemons);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }
    }
}
