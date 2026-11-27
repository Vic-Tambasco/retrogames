using RetroGames.Model.Model;
namespace RetroGames.Repository.Interfaces;
public interface IItemPedidoRepository
{
    Task InserirAsync(ItemPedido item);
    Task<IEnumerable<ItemPedido>> ListarPorPedidoAsync(int idPedido);
}