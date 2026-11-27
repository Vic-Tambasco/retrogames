using RetroGames.Model.Model;
namespace RetroGames.Repository.Interfaces;
public interface IPedidoRepository
{
    Task<int> InserirAsync(Pedido pedido);
    Task<Pedido?> BuscarPorIdAsync(int id);
    Task<IEnumerable<Pedido>> ListarPorUsuarioAsync(int idUsuario);
}