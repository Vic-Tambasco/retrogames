using MySqlConnector;
using RetroGames.Model.Model;
using RetroGames.Repository.Interfaces;

namespace RetroGames.Repository;

public class CondicaoJogoRepository : RepositoryBase, ICondicaoJogoRepository
{
    public CondicaoJogoRepository(RepositoryConfig config) : base(config) { }

    public async Task<List<CondicaoJogo>> ListarAsync()
    {
        var lista = new List<CondicaoJogo>();
        await using var conexao = await AbrirConexaoAsync();
        await using var comando = CriarComando(conexao, "SELECT co_condicao_jogo, ds_condicao_jogo FROM tb_condicao_jogo ORDER BY ds_condicao_jogo");
        await using var leitor = await comando.ExecuteReaderAsync();

        while (await leitor.ReadAsync())
        {
            lista.Add(new CondicaoJogo 
            { 
                Id = leitor.GetInt32("co_condicao_jogo"), 
                Descricao = leitor.GetString("ds_condicao_jogo") 
            });
        }

        return lista;
    }
}