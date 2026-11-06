using MySqlConnector;
using RetroGames.Model.Model;
using RetroGames.Repository.Interfaces;

namespace RetroGames.Repository;

public class JogoGeneroRepository : RepositoryBase, IJogoGeneroRepository
{
    public JogoGeneroRepository(RepositoryConfig config) : base(config) { }

    public async Task<List<JogoGenero>> ListarTodosAsync()
    {
        var lista = new List<JogoGenero>();
        await using var conexao = await AbrirConexaoAsync();
        await using var comando = CriarComando(conexao, "SELECT co_seq_jogo, co_seq_genero FROM tb_jogo_genero");
        await using var leitor = await comando.ExecuteReaderAsync();

        while (await leitor.ReadAsync())
        {
            // Ajustado para IdJogo e IdGenero
            lista.Add(new JogoGenero 
            { 
                IdJogo = leitor.GetInt32("co_seq_jogo"), 
                IdGenero = leitor.GetInt32("co_seq_genero") 
            });
        }

        return lista;
    }

    public async Task<List<JogoGenero>> ListarPorJogosAsync(List<int> jogosIds)
    {
        if (jogosIds == null || !jogosIds.Any()) return new List<JogoGenero>();

        var lista = new List<JogoGenero>();
        var idsFormatados = string.Join(",", jogosIds);
        
        await using var conexao = await AbrirConexaoAsync();
        await using var comando = CriarComando(conexao, $"SELECT co_seq_jogo, co_seq_genero FROM tb_jogo_genero WHERE co_seq_jogo IN ({idsFormatados})");
        await using var leitor = await comando.ExecuteReaderAsync();

        while (await leitor.ReadAsync())
        {
            // Ajustado para IdJogo e IdGenero
            lista.Add(new JogoGenero 
            { 
                IdJogo = leitor.GetInt32("co_seq_jogo"), 
                IdGenero = leitor.GetInt32("co_seq_genero") 
            });
        }

        return lista;
    }

    public async Task InserirAsync(int jogoId, List<int> generosIds)
    {
        if (generosIds == null || !generosIds.Any()) return;

        var valores = new List<string>();
        
        // 1. Monta o texto do SQL primeiro (forma mais segura)
        for (int i = 0; i < generosIds.Count; i++)
        {
            valores.Add($"(@jogoId, @gen{i})");
        }
        var sql = $"INSERT INTO tb_jogo_genero (co_seq_jogo, co_seq_genero) VALUES {string.Join(", ", valores)}";

        // 2. Cria o comando
        await using var conexao = await AbrirConexaoAsync();
        await using var comando = CriarComando(conexao, sql);

        // 3. Adiciona os parâmetros
        AdicionarParametro(comando, "@jogoId", jogoId);
        for (int i = 0; i < generosIds.Count; i++)
        {
            AdicionarParametro(comando, $"@gen{i}", generosIds[i]);
        }

        // 4. Executa
        await ExecutarGravacaoAsync(comando);
    }

    public async Task RemoverPorJogoAsync(int jogoId)
    {
        await using var conexao = await AbrirConexaoAsync();
        await using var comando = CriarComando(conexao, "DELETE FROM tb_jogo_genero WHERE co_seq_jogo = @jogoId");
        AdicionarParametro(comando, "@jogoId", jogoId);
        await ExecutarGravacaoAsync(comando);
    }
}