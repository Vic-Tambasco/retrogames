using Moq;
using RetroGames.Model.Model;
using RetroGames.Repository;
using RetroGames.Repository.Interfaces;
using RetroGames.Services;
using Xunit;

namespace RetroGames.WebApi.UnitTests.Services;

public class PlataformaServiceTests
{
    private readonly Mock<IPlataformaRepository> _mockPlataformaRepository;
    private readonly Mock<IRepositorySet> _mockRepositorySet;
    private readonly PlataformaService _plataformaService;

    public PlataformaServiceTests()
    {
        _mockPlataformaRepository = new Mock<IPlataformaRepository>();
        _mockRepositorySet = new Mock<IRepositorySet>();

        _mockRepositorySet.Setup(r => r.Plataforma).Returns(_mockPlataformaRepository.Object);

        _plataformaService = new PlataformaService(_mockRepositorySet.Object);
    }

    // ========== TESTE 1 ==========
    [Fact]
    public async Task BuscarPorId_IdInexistente_LancaNaoEncontrada()
    {
        // Arrange
        _mockPlataformaRepository.Setup(r => r.BuscarPorIdAsync(99)).ReturnsAsync((Plataforma?)null);

        // Act
        var excecao = await Assert.ThrowsAsync<BusinessRuleException>(
            () => _plataformaService.BuscarPorIdAsync(99)
        );

        // Assert
        Assert.Equal(BusinessErrors.PLATAFORMA_NAO_ENCONTRADA.Code, excecao.Erro.Code);
    }

    // ========== TESTE 2 ==========
    [Fact]
    public async Task Inserir_NomeJaCadastrado_LancaNomeDuplicadoENaoGrava()
    {
        // Arrange
        var plataforma = new Plataforma { Nome = "Super Nintendo", Fabricante = "Nintendo", AnoDeLancamento = 1990 };
        
        // O mock diz que JÁ EXISTE uma plataforma com esse nome
        _mockPlataformaRepository.Setup(r => r.ExisteComNomeAsync(plataforma.Nome, null)).ReturnsAsync(true);

        // Act
        var excecao = await Assert.ThrowsAsync<BusinessRuleException>(
            () => _plataformaService.InserirAsync(plataforma)
        );

        // Assert
        Assert.Equal(BusinessErrors.PLATAFORMA_NOME_DUPLICADO.Code, excecao.Erro.Code);
        
        // Verifica se o método InserirAsync do repositório NUNCA foi chamado
        _mockPlataformaRepository.Verify(r => r.InserirAsync(It.IsAny<Plataforma>()), Times.Never);
    }

    // ========== TESTE 3 ==========
    [Fact]
    public async Task Inserir_RepositoryLancaRegistroDuplicado_LancaNomeDuplicado()
    {
        // Arrange
        var plataforma = new Plataforma { Nome = "Super Nintendo", Fabricante = "Nintendo", AnoDeLancamento = 1990 };
        
        // O nome está livre
        _mockPlataformaRepository.Setup(r => r.ExisteComNomeAsync(plataforma.Nome, null)).ReturnsAsync(false);
        
        // Mas na hora de gravar, o repositório lança RegistroDuplicadoException (erro 1062 do MySQL)
        _mockPlataformaRepository.Setup(r => r.InserirAsync(plataforma))
            .ThrowsAsync(new RegistroDuplicadoException("Erro de duplicidade"));

        // Act
        var excecao = await Assert.ThrowsAsync<BusinessRuleException>(
            () => _plataformaService.InserirAsync(plataforma)
        );

        // Assert
        Assert.Equal(BusinessErrors.PLATAFORMA_NOME_DUPLICADO.Code, excecao.Erro.Code);
    }

    // ========== TESTE 4 ==========
    [Fact]
    public async Task Inserir_NomeLivre_DevolvePlataformaComIdGerado()
    {
        // Arrange
        var plataforma = new Plataforma { Nome = "PlayStation 5", Fabricante = "Sony", AnoDeLancamento = 2020 };
        
        _mockPlataformaRepository.Setup(r => r.ExisteComNomeAsync(plataforma.Nome, null)).ReturnsAsync(false);
        
        // Simulamos o banco gerando o ID 10
        _mockPlataformaRepository.Setup(r => r.InserirAsync(It.IsAny<Plataforma>()))
            .ReturnsAsync(10);

        // Act
        var resultado = await _plataformaService.InserirAsync(plataforma);

        // Assert
        Assert.Equal(10, resultado.Id);
        Assert.Equal("PlayStation 5", resultado.Nome);
        
        // Verifica se o repositório foi chamado para gravar
        _mockPlataformaRepository.Verify(r => r.InserirAsync(It.IsAny<Plataforma>()), Times.Once);
    }

    // ========== TESTE 5 ==========
    [Fact]
    public async Task Alterar_MantendoProprioNome_GravaComSucesso()
    {
        // Arrange
        var plataforma = new Plataforma { Id = 5, Nome = "Atari 2600", Fabricante = "Atari", AnoDeLancamento = 1977 };
        
        // Simula que a plataforma existe no banco
        _mockPlataformaRepository.Setup(r => r.BuscarPorIdAsync(plataforma.Id)).ReturnsAsync(plataforma);
        
        // Simula que a checagem de nome (ignorando o próprio ID) retorna FALSE (não há duplicado)
        _mockPlataformaRepository.Setup(r => r.ExisteComNomeAsync(plataforma.Nome, plataforma.Id)).ReturnsAsync(false);

        // Act
        var resultado = await _plataformaService.AlterarAsync(plataforma);

        // Assert
        Assert.Equal("Atari 2600", resultado.Nome);
        _mockPlataformaRepository.Verify(r => r.AlterarAsync(It.IsAny<Plataforma>()), Times.Once);
    }

    // ========== TESTE 6 ==========
    [Fact]
    public async Task Excluir_PlataformaComJogos_LancaPossuiJogosENaoExclui()
    {
        // Arrange
        int id = 1;
        var plataforma = new Plataforma { Id = id, Nome = "PlayStation 1" };
        
        // Simula que a plataforma existe
        _mockPlataformaRepository.Setup(r => r.BuscarPorIdAsync(id)).ReturnsAsync(plataforma);
        
        // Mas ela tem jogos vinculados
        _mockPlataformaRepository.Setup(r => r.PossuiJogosAsync(id)).ReturnsAsync(true);

        // Act
        var excecao = await Assert.ThrowsAsync<BusinessRuleException>(
            () => _plataformaService.ExcluirAsync(id)
        );

        // Assert
        Assert.Equal(BusinessErrors.PLATAFORMA_POSSUI_JOGOS.Code, excecao.Erro.Code);
        
        // Garante que a exclusão NUNCA foi chamada
        _mockPlataformaRepository.Verify(r => r.ExcluirAsync(It.IsAny<int>()), Times.Never);
    }
}