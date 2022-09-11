using Pokemon_Api.Models;

namespace Pokemon_Api.Services
{
    public interface IPokemonService
    {
        Task<List<Pokemon>> GetPokemonsAsync(CancellationToken cancellationToken);
        Task<List<Pokemon>> GetPokemonsFilteredAsync(string term, CancellationToken cancellationToken);
    }
}