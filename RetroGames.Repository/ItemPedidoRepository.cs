using MySqlConnector;
using RetroGames.Model.Model;
using RetroGames.Repository.Interfaces;

namespace RetroGames.Repository;

public class ItemPedidoRepository : RepositoryBase, IItemPedidoRepository
{
    public ItemPedidoRepository(RepositoryConfig config) : base(config) { }

    public async Task InserirAsync(ItemPedido item)
    {
        using var conexao = await AbrirConexaoAsync();
        var sql = @"INSERT INTO tb_item_pedido (qt_item, vl_unitario, co_pedido, co_jogo) 
                    VALUES (@qt, @valor, @idPedido, @idJogo);";
        using var cmd = new MySqlCommand(sql, conexao);
        cmd.Parameters.AddWithValue("@qt", item.Quantidade);
        cmd.Parameters.AddWithValue("@valor", item.ValorUnitario);
        cmd.Parameters.AddWithValue("@idPedido", item.IdPedido);
        cmd.Parameters.AddWithValue("@idJogo", item.IdJogo);
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task<IEnumerable<ItemPedido>> ListarPorPedidoAsync(int idPedido) { /* Implementar Select */ return new List<ItemPedido>(); }
}