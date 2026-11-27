using Moq;
using RetroGames.Model.Model;
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

    [Fact]
    public async Task BuscarPorId_IdInexistente_LancaNaoEncontrada()
    {
        // Arrange
        // O ? depois de Plataforma avisa ao C# que o retorno pode ser nulo, sumindo com o aviso
        _mockPlataformaRepository.Setup(r => r.BuscarPorIdAsync(99)).ReturnsAsync((Plataforma?)null);

        // Act
        var excecao = await Assert.ThrowsAsync<BusinessRuleException>(
            () => _plataformaService.BuscarPorIdAsync(99)
        );

        // Assert
        Assert.Equal(BusinessErrors.PLATAFORMA_NAO_ENCONTRADA.Code, excecao.Erro.Code);
    }
}