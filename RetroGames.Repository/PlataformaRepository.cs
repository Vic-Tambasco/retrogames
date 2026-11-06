using MySqlConnector;
using RetroGames.Repository.Interfaces;
using RetroGames.Model.Model;

namespace RetroGames.Repository;

public class PlataformaRepository : RepositoryBase, IPlataformaRepository
{
    public PlataformaRepository(RepositoryConfig config) : base(config) { }

    public async Task<List<Plataforma>> ListarAsync()
    {
        var lista = new List<Plataforma>();
        await using var conexao = await AbrirConexaoAsync();
        await using var comando = CriarComando(conexao,
            "SELECT co_seq_plataforma, no_plataforma, ds_fabricante, nu_ano_lancamento " +
            "FROM tb_plataforma ORDER BY no_plataforma");
        await using var leitor = await comando.ExecuteReaderAsync();

        while (await leitor.ReadAsync())
            lista.Add(Mapear(leitor));

        return lista;
    }

    public async Task<Plataforma?> BuscarPorIdAsync(int id)
    {
        await using var conexao = await AbrirConexaoAsync();
        await using var comando = CriarComando(conexao,
            "SELECT co_seq_plataforma, no_plataforma, ds_fabricante, nu_ano_lancamento " +
            "FROM tb_plataforma WHERE co_seq_plataforma = @id");
        AdicionarParametro(comando, "@id", id);
        await using var leitor = await comando.ExecuteReaderAsync();

        return await leitor.ReadAsync() ? Mapear(leitor) : null;
    }

    public async Task<bool> ExisteComNomeAsync(string nome, int? ignorarId = null)
    {
        await using var conexao = await AbrirConexaoAsync();
        var sql = "SELECT COUNT(*) FROM tb_plataforma WHERE no_plataforma = @nome";
        if (ignorarId.HasValue) sql += " AND co_seq_plataforma <> @id";

        await using var comando = CriarComando(conexao, sql);
        AdicionarParametro(comando, "@nome", nome);
        if (ignorarId.HasValue) AdicionarParametro(comando, "@id", ignorarId.Value);

        var total = Convert.ToInt32(await comando.ExecuteScalarAsync());
        return total > 0;
    }

    public async Task<bool> PossuiJogosAsync(int id)
    {
        await using var conexao = await AbrirConexaoAsync();
        // ATENÇÃO AQUI: A coluna na tb_jogo chama-se co_plataforma (verificado no seu script)
        await using var comando = CriarComando(conexao,
            "SELECT COUNT(*) FROM tb_jogo WHERE co_plataforma = @id");
        AdicionarParametro(comando, "@id", id);

        var total = Convert.ToInt32(await comando.ExecuteScalarAsync());
        return total > 0;
    }

    public async Task<int> InserirAsync(Plataforma plataforma)
    {
        await using var conexao = await AbrirConexaoAsync();
        await using var comando = CriarComando(conexao,
            "INSERT INTO tb_plataforma (no_plataforma, ds_fabricante, nu_ano_lancamento) " +
            "VALUES (@nome, @fabricante, @ano)");
        
        AdicionarParametro(comando, "@nome", plataforma.Nome);
        AdicionarParametro(comando, "@fabricante", plataforma.Fabricante);
        AdicionarParametro(comando, "@ano", plataforma.AnoDeLancamento);

        // Certifique-se de que este método ExecutarGravacaoAsync foi o nome que você deu na Etapa 1
        await ExecutarGravacaoAsync(comando); 
        
        // Retorna o ID gerado pelo banco de dados
        return (int)comando.LastInsertedId;
    }

    public async Task AlterarAsync(Plataforma plataforma)
    {
        await using var conexao = await AbrirConexaoAsync();
        await using var comando = CriarComando(conexao,
            "UPDATE tb_plataforma SET no_plataforma = @nome, ds_fabricante = @fabricante, " +
            "nu_ano_lancamento = @ano WHERE co_seq_plataforma = @id");
        
        AdicionarParametro(comando, "@nome", plataforma.Nome);
        AdicionarParametro(comando, "@fabricante", plataforma.Fabricante);
        AdicionarParametro(comando, "@ano", plataforma.AnoDeLancamento);
        AdicionarParametro(comando, "@id", plataforma.Id);

        await ExecutarGravacaoAsync(comando);
    }

    public async Task ExcluirAsync(int id)
    {
        await using var conexao = await AbrirConexaoAsync();
        await using var comando = CriarComando(conexao,
            "DELETE FROM tb_plataforma WHERE co_seq_plataforma = @id");
        
        AdicionarParametro(comando, "@id", id);

        await ExecutarGravacaoAsync(comando);
    }

    // Método auxiliar para não repetir código de conversão
    private static Plataforma Mapear(MySqlDataReader leitor) => new()
    {
        Id = leitor.GetInt32("co_seq_plataforma"),
        Nome = leitor.GetString("no_plataforma"),
        Fabricante = leitor.GetString("ds_fabricante"), // Coluna correta do seu banco
        AnoDeLancamento = leitor.GetInt32("nu_ano_lancamento")
    };
}