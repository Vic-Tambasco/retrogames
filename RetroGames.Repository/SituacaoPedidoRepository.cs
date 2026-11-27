using MySqlConnector;
using RetroGames.Model.Model;
using RetroGames.Repository.Interfaces;

namespace RetroGames.Repository;

public class SituacaoPedidoRepository : RepositoryBase, ISituacaoPedidoRepository
{
    public SituacaoPedidoRepository(RepositoryConfig config) : base(config) { }

    public async Task<IEnumerable<SituacaoPedido>> ListarAsync()
    {
        var lista = new List<SituacaoPedido>();
        using var conexao = await AbrirConexaoAsync();
        var sql = "SELECT co_situacao_pedido, ds_situacao_pedido FROM tb_situacao_pedido";
        using var cmd = new MySqlCommand(sql, conexao);
        using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            lista.Add(new SituacaoPedido
            {
                Id = reader.GetInt32("co_situacao_pedido"),
                Descricao = reader.GetString("ds_situacao_pedido")
            });
        }
        return lista;
    }
}