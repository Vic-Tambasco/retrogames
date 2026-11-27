using RetroGames.Model.Model;

namespace RetroGames.Services;

public class PedidoDetalheServiceModel
{
    public Pedido Pedido { get; set; } = null!;
    public string DescricaoSituacao { get; set; } = string.Empty;
    public IEnumerable<ItemPedidoDetalhe> Itens { get; set; } = new List<ItemPedidoDetalhe>();
}

public class ItemPedidoDetalhe
{
    public string NomeJogo { get; set; } = string.Empty;
    public int Quantidade { get; set; }
    public decimal ValorUnitario { get; set; }
}