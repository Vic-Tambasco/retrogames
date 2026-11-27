using Moq;
using RetroGames.Model.Model;
using RetroGames.Repository.Interfaces;
using RetroGames.Services;
using Xunit;
using System.Collections.Generic;
using System.Linq;

namespace RetroGames.WebApi.UnitTests.Services;

public class CondicaoJogoServiceTests
{
    private readonly Mock<ICondicaoJogoRepository> _mockCondicaoRepo;
    private readonly Mock<IRepositorySet> _mockRepositorySet;
    private readonly CondicaoJogoService _condicaoService;

    public CondicaoJogoServiceTests()
    {
        _mockCondicaoRepo = new Mock<ICondicaoJogoRepository>();
        _mockRepositorySet = new Mock<IRepositorySet>();
        _mockRepositorySet.Setup(r => r.CondicaoJogo).Returns(_mockCondicaoRepo.Object);

        _condicaoService = new CondicaoJogoService(_mockRepositorySet.Object);
    }

    [Fact]
    public async Task Listar_RepositorioRetornaCondicoes_DevolveAsMesmasCondicoes()
    {
        var condicoes = new List<CondicaoJogo>
        {
            new CondicaoJogo { Id = 1, Descricao = "Novo" },
            new CondicaoJogo { Id = 2, Descricao = "Usado" }
        };
        _mockCondicaoRepo.Setup(r => r.ListarAsync()).ReturnsAsync(condicoes);

        var resultado = (await _condicaoService.ListarAsync()).ToList();

        Assert.Equal(2, resultado.Count);
        Assert.Equal("Novo", resultado[0].Descricao);
        Assert.Equal("Usado", resultado[1].Descricao);
    }
}