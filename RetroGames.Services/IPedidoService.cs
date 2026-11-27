using RetroGames.Model.Model;

namespace RetroGames.Services;

public class PedidoRequest
{
    public List<ItemPedidoRequest> Itens { get; set; } = new();
}

public class ItemPedidoRequest
{
    public int IdJogo { get; set; }
    public int Quantidade { get; set; }
}

public interface IPedidoService
{
    Task<Pedido> FinalizarCompraAsync(int idUsuario, PedidoRequest request);
    Task<IEnumerable<PedidoDetalheServiceModel>> ListarMeusPedidosAsync(int idUsuario);
}
