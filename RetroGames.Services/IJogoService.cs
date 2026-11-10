using System.Collections.Generic;
using System.Threading.Tasks;
using RetroGames.Model.Model;

namespace RetroGames.Services;

public interface IJogoService
{
    Task<(IEnumerable<JogoDetalheServiceModel> Items, int Total)> ListarAsync(JogoFiltro filtro);
    Task<JogoDetalheServiceModel> BuscarPorIdAsync(int id);
    Task<Jogo> InserirAsync(Jogo jogo, IEnumerable<int> idsGeneros);
    Task<Jogo> AlterarAsync(Jogo jogo, IEnumerable<int> idsGeneros);
    Task DesativarAsync(int id);
}