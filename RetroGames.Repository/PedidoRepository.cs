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

    public async Task<Pedido?> BuscarPorIdAsync(int id)
    {
        using var conexao = await AbrirConexaoAsync();
        const string sql = @"SELECT co_seq_pedido, dt_pedido, vl_total, co_usuario, co_situacao_pedido
                             FROM tb_pedido
                             WHERE co_seq_pedido = @id";
        using var cmd = new MySqlCommand(sql, conexao);
        cmd.Parameters.AddWithValue("@id", id);

        using var reader = await cmd.ExecuteReaderAsync();
        return await reader.ReadAsync() ? Mapear(reader) : null;
    }

    public async Task<IEnumerable<Pedido>> ListarPorUsuarioAsync(int idUsuario)
    {
        var pedidos = new List<Pedido>();
        using var conexao = await AbrirConexaoAsync();
        const string sql = @"SELECT co_seq_pedido, dt_pedido, vl_total, co_usuario, co_situacao_pedido
                             FROM tb_pedido
                             WHERE co_usuario = @idUsuario
                             ORDER BY dt_pedido DESC, co_seq_pedido DESC";
        using var cmd = new MySqlCommand(sql, conexao);
        cmd.Parameters.AddWithValue("@idUsuario", idUsuario);

        using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
            pedidos.Add(Mapear(reader));

        return pedidos;
    }

    private static Pedido Mapear(MySqlDataReader reader) => new()
    {
        Id = reader.GetInt32("co_seq_pedido"),
        DataPedido = reader.GetDateTime("dt_pedido"),
        Total = reader.GetDecimal("vl_total"),
        IdUsuario = reader.GetInt32("co_usuario"),
        IdSituacaoPedido = reader.GetInt32("co_situacao_pedido")
    };
}