using System.Threading;
using Pokemon_Api.Models;

namespace Pokemon_Api.Services
{
    public class PokemonService : IPokemonService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public PokemonService(IConfiguration configuration, HttpClient httpClient)
        {
            _configuration = configuration;
            _httpClient = httpClient;
        }

        public async Task<List<Pokemon>> GetPokemonsAsync(CancellationToken cancellationToken)
        {
            var pokemons = await _httpClient.GetFromJsonAsync<PokemonJsonResponse>(
                $"{_configuration.GetValue<string>("Pokemon:ApiBaseUrl")}/{_configuration.GetValue<string>("Pokemon:ListEndpoint")}", cancellationToken);


            if (pokemons is null or { Results.Count: 0 })
            {
                throw new InvalidOperationException("Something went wrong while getting the list of Pokemons");
            }

            return pokemons.Results;
        }

        public async Task<List<Pokemon>> GetPokemonsFilteredAsync(string term, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(term))
            {
                throw new InvalidOperationException("The filter term is required");
            }

            var pokemons = await _httpClient.GetFromJsonAsync<PokemonJsonResponse>(
                $"{_configuration.GetValue<string>("Pokemon:ApiBaseUrl")}/{_configuration.GetValue<string>("Pokemon:ListEndpoint")}", cancellationToken);


            if (pokemons is null)
            {
                throw new InvalidOperationException("Something went wrong while getting the list of Pokemons");
            }

            pokemons.Results = pokemons.Results.Where(x => x.Name.Contains(term)).ToList();

            return pokemons.Results;
        }
    }
}
