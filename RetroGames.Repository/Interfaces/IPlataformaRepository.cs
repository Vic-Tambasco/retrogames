using RetroGames.Model.Model; 

namespace RetroGames.Repository.Interfaces;

public interface IPlataformaRepository
{
    Task<List<Plataforma>> ListarAsync();
    Task<Plataforma?> BuscarPorIdAsync(int id);
    Task<bool> ExisteComNomeAsync(string nome, int? ignorarId = null);
    Task<bool> PossuiJogosAsync(int id);
    Task<int> InserirAsync(Plataforma plataforma);
    Task AlterarAsync(Plataforma plataforma);
    Task ExcluirAsync(int id);
}