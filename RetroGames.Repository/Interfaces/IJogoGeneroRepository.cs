using RetroGames.Model.Model;

namespace RetroGames.Repository.Interfaces;

public interface IJogoGeneroRepository
{
    Task<List<JogoGenero>> ListarTodosAsync();
    Task<List<JogoGenero>> ListarPorJogosAsync(List<int> jogosIds);
    Task InserirAsync(int jogoId, List<int> generosIds);
    Task RemoverPorJogoAsync(int jogoId);
}