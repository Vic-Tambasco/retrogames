using MySqlConnector;
using RetroGames.Model.Model;
using RetroGames.Repository.Interfaces;

namespace RetroGames.Repository;

public class PedidoRepository : RepositoryBase, IPedidoRepository
{
    public PedidoRepository(RepositoryConfig config) : base(config) { }

    public async Task<int> InserirAsync(Pedido pedido)
    {
        using var conexao = await AbrirConexaoAsync();
        var sql = @"INSERT INTO tb_pedido (dt_pedido, vl_total, co_usuario, co_situacao_pedido) 
                    VALUES (@data, @total, @idUsuario, @idSituacao); SELECT LAST_INSERT_ID();";
        using var cmd = new MySqlCommand(sql, conexao);
        cmd.Parameters.AddWithValue("@data", pedido.DataPedido);
        cmd.Parameters.AddWithValue("@total", pedido.Total);
        cmd.Parameters.AddWithValue("@idUsuario", pedido.IdUsuario);
        cmd.Parameters.AddWithValue("@idSituacao", pedido.IdSituacaoPedido);
        return Convert.ToInt32(await cmd.ExecuteScalarAsync());
    }

    public async Task<Pedido?> BuscarPorIdAsync(int id) { /* Implementar Select */ return null; }
    public async Task<IEnumerable<Pedido>> ListarPorUsuarioAsync(int idUsuario) { /* Implementar Select */ return new List<Pedido>(); }
}