using Moq;
using RetroGames.Model.Model;
using RetroGames.Repository.Interfaces;
using RetroGames.Services;
using Xunit;
using System.Collections.Generic;
using System.Linq;

namespace RetroGames.WebApi.UnitTests.Services;

public class JogoServiceTests
{
    private readonly Mock<IJogoRepository> _mockJogoRepo;
    private readonly Mock<IPlataformaRepository> _mockPlataformaRepo;
    private readonly Mock<ICondicaoJogoRepository> _mockCondicaoRepo;
    private readonly Mock<IGeneroRepository> _mockGeneroRepo;
    private readonly Mock<IJogoGeneroRepository> _mockJogoGeneroRepo;
    private readonly Mock<IRepositorySet> _mockRepositorySet;
    private readonly Mock<IUsuarioAtualService> _mockUsuarioAtual;

    private readonly JogoService _jogoService;

    public JogoServiceTests()
    {
        _mockJogoRepo = new Mock<IJogoRepository>();
        _mockPlataformaRepo = new Mock<IPlataformaRepository>();
        _mockCondicaoRepo = new Mock<ICondicaoJogoRepository>();
        _mockGeneroRepo = new Mock<IGeneroRepository>();
        _mockJogoGeneroRepo = new Mock<IJogoGeneroRepository>();
        _mockRepositorySet = new Mock<IRepositorySet>();
        _mockUsuarioAtual = new Mock<IUsuarioAtualService>();

        _mockRepositorySet.Setup(r => r.Jogo).Returns(_mockJogoRepo.Object);
        _mockRepositorySet.Setup(r => r.Plataforma).Returns(_mockPlataformaRepo.Object);
        _mockRepositorySet.Setup(r => r.CondicaoJogo).Returns(_mockCondicaoRepo.Object);
        _mockRepositorySet.Setup(r => r.Genero).Returns(_mockGeneroRepo.Object);
        _mockRepositorySet.Setup(r => r.JogoGenero).Returns(_mockJogoGeneroRepo.Object);

        _jogoService = new JogoService(_mockRepositorySet.Object, _mockUsuarioAtual.Object);
    }

    // ========== TESTES DE INSERÇÃO ==========

    [Fact]
    public async Task Inserir_PlataformaInexistente_LancaNaoEncontrada()
    {
        var jogo = new Jogo { IdPlataforma = 99, IdCondicaoJogo = 1 };
        _mockPlataformaRepo.Setup(r => r.BuscarPorIdAsync(99)).ReturnsAsync((Plataforma?)null);
        _mockCondicaoRepo.Setup(r => r.ListarAsync()).ReturnsAsync(new List<CondicaoJogo> { new CondicaoJogo { Id = 1 } });
        _mockGeneroRepo.Setup(r => r.ListarAsync()).ReturnsAsync(new List<Genero> { new Genero { Id = 1 } });

        var excecao = await Assert.ThrowsAsync<BusinessRuleException>(
            () => _jogoService.InserirAsync(jogo, new List<int> { 1 })
        );

        Assert.Equal(BusinessErrors.PLATAFORMA_NAO_ENCONTRADA.Code, excecao.Erro.Code);
        _mockJogoRepo.Verify(r => r.InserirAsync(It.IsAny<Jogo>()), Times.Never);
    }

    [Fact]
    public async Task Inserir_CondicaoInexistente_LancaNaoEncontrada()
    {
        var jogo = new Jogo { IdPlataforma = 1, IdCondicaoJogo = 99 };
        _mockPlataformaRepo.Setup(r => r.BuscarPorIdAsync(1)).ReturnsAsync(new Plataforma { Id = 1 });
        _mockCondicaoRepo.Setup(r => r.ListarAsync()).ReturnsAsync(new List<CondicaoJogo>());
        _mockGeneroRepo.Setup(r => r.ListarAsync()).ReturnsAsync(new List<Genero> { new Genero { Id = 1 } });

        var excecao = await Assert.ThrowsAsync<BusinessRuleException>(
            () => _jogoService.InserirAsync(jogo, new List<int> { 1 })
        );

        Assert.Equal(BusinessErrors.CONDICAO_NAO_ENCONTRADA.Code, excecao.Erro.Code);
        _mockJogoRepo.Verify(r => r.InserirAsync(It.IsAny<Jogo>()), Times.Never);
    }

    [Fact]
    public async Task Inserir_GeneroInexistente_LancaNaoEncontrado()
    {
        var jogo = new Jogo { IdPlataforma = 1, IdCondicaoJogo = 1 };
        _mockPlataformaRepo.Setup(r => r.BuscarPorIdAsync(1)).ReturnsAsync(new Plataforma { Id = 1 });
        _mockCondicaoRepo.Setup(r => r.ListarAsync()).ReturnsAsync(new List<CondicaoJogo> { new CondicaoJogo { Id = 1 } });
        _mockGeneroRepo.Setup(r => r.ListarAsync()).ReturnsAsync(new List<Genero>());

        var excecao = await Assert.ThrowsAsync<BusinessRuleException>(
            () => _jogoService.InserirAsync(jogo, new List<int> { 99 })
        );

        Assert.Equal(BusinessErrors.GENERO_NAO_ENCONTRADO.Code, excecao.Erro.Code);
        _mockJogoRepo.Verify(r => r.InserirAsync(It.IsAny<Jogo>()), Times.Never);
    }

    // ========== TESTES DE LISTAGEM (LINQ) ==========

    [Fact]
    public async Task Listar_FiltrandoPorGenero_EnviaGenerosAoRepositorio()
    {
        var filtro = new JogoFiltro { GeneroIds = new List<int> { 5 } };

        _mockJogoRepo.Setup(r => r.ListarAsync(
                It.IsAny<string>(),
                It.IsAny<int?>(),
                It.IsAny<List<int>>(),
                It.IsAny<bool>(),
                It.IsAny<int>(),
                It.IsAny<int>()))
            .ReturnsAsync((new List<Jogo>(), 0));

        await _jogoService.ListarAsync(filtro);

        _mockJogoRepo.Verify(r => r.ListarAsync(
                It.IsAny<string>(),
                It.IsAny<int?>(),
                It.Is<List<int>>(ids => ids != null && ids.SequenceEqual(new List<int> { 5 })),
                It.IsAny<bool>(),
                It.IsAny<int>(),
                It.IsAny<int>()),
            Times.Once);
    }

    [Fact]
    public async Task Listar_SemGenero_NaoConsultaVinculosENaoRestringeIds()
    {
        var filtro = new JogoFiltro { Nome = "Mario" };

        _mockJogoRepo.Setup(r => r.ListarAsync(
                It.IsAny<string>(),
                It.IsAny<int?>(),
                It.IsAny<List<int>>(),
                It.IsAny<bool>(),
                It.IsAny<int>(),
                It.IsAny<int>()))
            .ReturnsAsync((new List<Jogo>(), 0));

        await _jogoService.ListarAsync(filtro);

        _mockJogoRepo.Verify(r => r.ListarAsync(
                It.IsAny<string>(),
                It.IsAny<int?>(),
                It.Is<List<int>>(ids => ids == null || !ids.Any()),
                It.IsAny<bool>(),
                It.IsAny<int>(),
                It.IsAny<int>()),
            Times.Once);
        _mockJogoGeneroRepo.Verify(r => r.ListarPorJogosAsync(It.IsAny<List<int>>()), Times.Never);
    }

    [Fact]
    public async Task Listar_PaginaComJogos_MontaPlataformaECondicao()
    {
        var filtro = new JogoFiltro { Pagina = 1 };
        var jogo = new Jogo { Id = 1, IdPlataforma = 10, IdCondicaoJogo = 20, Nome = "Sonic" };

        _mockJogoRepo.Setup(r => r.ListarAsync(
                It.IsAny<string>(),
                It.IsAny<int?>(),
                It.IsAny<List<int>>(),
                It.IsAny<bool>(),
                It.IsAny<int>(),
                It.IsAny<int>()))
            .ReturnsAsync((new List<Jogo> { jogo }, 1));

        _mockPlataformaRepo.Setup(r => r.ListarAsync()).ReturnsAsync(new List<Plataforma> { new Plataforma { Id = 10, Nome = "Mega Drive" } });
        _mockCondicaoRepo.Setup(r => r.ListarAsync()).ReturnsAsync(new List<CondicaoJogo> { new CondicaoJogo { Id = 20, Descricao = "Usado" } });
        _mockGeneroRepo.Setup(r => r.ListarAsync()).ReturnsAsync(new List<Genero>());
        _mockJogoGeneroRepo.Setup(r => r.ListarPorJogosAsync(It.IsAny<List<int>>())).ReturnsAsync(new List<JogoGenero>());

        var (itens, total) = await _jogoService.ListarAsync(filtro);
        var resultado = itens.First();

        Assert.Equal(1, total);
        Assert.Equal("Mega Drive", resultado.NomePlataforma);
        Assert.Equal("Usado", resultado.DescricaoCondicao);
    }

    // ========== TESTES DE BUSCA E EXCLUSÃO ==========

    [Fact]
    public async Task BuscarPorId_IdInexistente_LancaJogoNaoEncontrado()
    {
        _mockJogoRepo.Setup(r => r.BuscarPorIdAsync(99)).ReturnsAsync((Jogo?)null);

        var excecao = await Assert.ThrowsAsync<BusinessRuleException>(
            () => _jogoService.BuscarPorIdAsync(99)
        );

        Assert.Equal(BusinessErrors.JOGO_NAO_ENCONTRADO.Code, excecao.Erro.Code);
    }

    [Fact]
    public async Task Desativar_IdInexistente_LancaJogoNaoEncontradoENaoDesativa()
    {
        _mockJogoRepo.Setup(r => r.BuscarPorIdAsync(99)).ReturnsAsync((Jogo?)null);

        var excecao = await Assert.ThrowsAsync<BusinessRuleException>(
            () => _jogoService.DesativarAsync(99)
        );

        Assert.Equal(BusinessErrors.JOGO_NAO_ENCONTRADO.Code, excecao.Erro.Code);
        _mockJogoRepo.Verify(r => r.DesativarAsync(It.IsAny<int>()), Times.Never);
    }
}