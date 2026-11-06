using RetroGames.Model.Model;

namespace RetroGames.Repository.Interfaces;

public interface IJogoRepository
{
    Task<(List<Jogo> Jogos, int Total)> ListarAsync(string? nome, int? plataformaId, List<int>? generoIds, bool incluirInativos, int pagina, int tamanhoPagina);
    Task<Jogo?> BuscarPorIdAsync(int id);
    Task<int> InserirAsync(Jogo jogo);
    Task AlterarAsync(Jogo jogo);
    Task DesativarAsync(int id);
}