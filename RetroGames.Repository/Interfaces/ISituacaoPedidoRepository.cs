using RetroGames.Model.Model;
namespace RetroGames.Repository.Interfaces;
public interface ISituacaoPedidoRepository
{
    Task<IEnumerable<SituacaoPedido>> ListarAsync();
}