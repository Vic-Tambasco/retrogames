using System.Collections.Generic;
using System.Threading.Tasks;
using RetroGames.Model.Model;

namespace RetroGames.Services;

public interface IJogoService
{
    // O filtro pode ser um objeto ou parâmetros separados. Exemplo com objeto:
    Task<(IEnumerable<JogoDetalheServiceModel> Items, int Total)> ListarAsync(JogoFiltro filtro);
    Task<JogoDetalheServiceModel> BuscarPorIdAsync(int id);
    Task<Jogo> InserirAsync(Jogo jogo, IEnumerable<int> idsGeneros);
    Task<Jogo> AlterarAsync(Jogo jogo, IEnumerable<int> idsGeneros);
    Task DesativarAsync(int id);
}