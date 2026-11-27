using Moq;
using RetroGames.Model.Model;
using RetroGames.Repository.Interfaces;
using RetroGames.Services;
using Xunit;
using System.Collections.Generic;
using System.Linq;

namespace RetroGames.WebApi.UnitTests.Services;

public class GeneroServiceTests
{
    private readonly Mock<IGeneroRepository> _mockGeneroRepo;
    private readonly Mock<IRepositorySet> _mockRepositorySet;
    private readonly GeneroService _generoService;

    public GeneroServiceTests()
    {
        _mockGeneroRepo = new Mock<IGeneroRepository>();
        _mockRepositorySet = new Mock<IRepositorySet>();
        _mockRepositorySet.Setup(r => r.Genero).Returns(_mockGeneroRepo.Object);

        _generoService = new GeneroService(_mockRepositorySet.Object);
    }

    [Fact]
    public async Task Listar_RepositorioRetornaGeneros_DevolveOsMesmosGeneros()
    {
        var generos = new List<Genero>
        {
            new Genero { Id = 1, Nome = "Plataforma" },
            new Genero { Id = 2, Nome = "RPG" }
        };
        _mockGeneroRepo.Setup(r => r.ListarAsync()).ReturnsAsync(generos);

        var resultado = (await _generoService.ListarAsync()).ToList();

        Assert.Equal(2, resultado.Count);
        Assert.Equal("Plataforma", resultado[0].Nome);
        Assert.Equal("RPG", resultado[1].Nome);
    }
}