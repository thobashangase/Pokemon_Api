using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Pokemon_Api.Controllers;
using Pokemon_Api.Models;
using Pokemon_Api.Services;

namespace Pokemon_Api.Tests
{
    public  class PokemonControllerTest
    {
        private readonly List<Pokemon> samplePokemonsResponse = new()
        {
            new Pokemon
            {
                Name = "Bulbasaur",
                Url = "https://pokeapi.co/api/v2/pokemon/1/"
            },
            new Pokemon
            {
                Name = "Ivysaur",
                Url = "https://pokeapi.co/api/v2/pokemon/2/"
            }
        };

        private readonly Mock<ILogger<PokemonController>> mockLogger;

        private readonly Mock<IPokemonService> mockPokemonService;

        public PokemonControllerTest()
        {
            mockPokemonService ??= new Mock<IPokemonService>();
            mockLogger ??= new Mock<ILogger<PokemonController>>();
        }

        [Fact]
        public void GetPokemons_ListOfPokemons_PokemonsExist()
        {
            //Arrange
            mockPokemonService.Setup(x => x.GetPokemonsAsync(CancellationToken.None)).Returns(Task.FromResult(samplePokemonsResponse));
            var controller = new PokemonController(mockLogger.Object, mockPokemonService.Object);

            //Act
            var actionResult = controller.GetAsync();
            var result = actionResult.Result as OkObjectResult;
            var actual = result!.Value as List<Pokemon>;


            //Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(samplePokemonsResponse, actual);
        }

        [Fact]
        public void GetPokemonsFiltered_ListOfPokemons_FilteredPokemonsExist()
        {
            //Arrange
            var term = "Bulb";
            var filteredResponse = samplePokemonsResponse.Take(1).ToList();
            mockPokemonService.Setup(x => x.GetPokemonsFilteredAsync(term, CancellationToken.None)).Returns(Task.FromResult(filteredResponse));
            var controller = new PokemonController(mockLogger.Object, mockPokemonService.Object);

            //Act            
            var actionResult = controller.GetFilteredAsync(term);
            var result = actionResult.Result as OkObjectResult;
            var actual = result!.Value as List<Pokemon>;


            //Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(filteredResponse, actual);
        }

        [Fact]
        public void GetPokemonsFiltered_ListOfPokemons_EmptySearchTermReturnsBadRequestStatus()
        {
            //Arrange
            var term = "";
            var filteredResponse = samplePokemonsResponse.Take(1).ToList();
            mockPokemonService.Setup(x => x.GetPokemonsFilteredAsync(term, CancellationToken.None)).Returns(Task.FromResult(filteredResponse));
            var controller = new PokemonController(mockLogger.Object, mockPokemonService.Object);

            //Act            
            var actionResult = controller.GetFilteredAsync(term);
            var result = actionResult.Result;


            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}
