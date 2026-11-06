using MySqlConnector;
using RetroGames.Model.Model;
using RetroGames.Repository.Interfaces;

namespace RetroGames.Repository;

public class JogoRepository : RepositoryBase, IJogoRepository
{
    public JogoRepository(RepositoryConfig config) : base(config) { }

    public async Task<(List<Jogo> Jogos, int Total)> ListarAsync(string? nome, int? plataformaId, List<int>? generoIds, bool incluirInativos, int pagina, int tamanhoPagina)
    {
        var jogos = new List<Jogo>();
        var whereClauses = new List<string>();
        
        if (!incluirInativos) whereClauses.Add("st_ativo = 1");
        if (!string.IsNullOrWhiteSpace(nome)) whereClauses.Add("no_jogo LIKE @nome");
        if (plataformaId.HasValue) whereClauses.Add("co_plataforma = @plataformaId");
        
        if (generoIds != null && generoIds.Any())
        {
            var generosFormatados = string.Join(",", generoIds);
            whereClauses.Add($"co_seq_jogo IN (SELECT co_seq_jogo FROM tb_jogo_genero WHERE co_seq_genero IN ({generosFormatados}))");
        }

        var whereSql = whereClauses.Any() ? "WHERE " + string.Join(" AND ", whereClauses) : "";

        await using var conexao = await AbrirConexaoAsync();

        // 1. Conta o total
        await using var comandoTotal = CriarComando(conexao, $"SELECT COUNT(*) FROM tb_jogo {whereSql}");
        AdicionarParametros(comandoTotal, nome, plataformaId);
        var total = Convert.ToInt32(await comandoTotal.ExecuteScalarAsync());

        // 2. Busca os dados paginados
        var sqlDados = $"SELECT * FROM tb_jogo {whereSql} ORDER BY no_jogo LIMIT @limit OFFSET @offset";
        await using var comandoDados = CriarComando(conexao, sqlDados);
        AdicionarParametros(comandoDados, nome, plataformaId);
        AdicionarParametro(comandoDados, "@limit", tamanhoPagina);
        AdicionarParametro(comandoDados, "@offset", (pagina - 1) * tamanhoPagina);

        await using var leitor = await comandoDados.ExecuteReaderAsync();
        while (await leitor.ReadAsync())
            jogos.Add(Mapear(leitor));

        return (jogos, total);
    }

    public async Task<Jogo?> BuscarPorIdAsync(int id)
    {
        await using var conexao = await AbrirConexaoAsync();
        await using var comando = CriarComando(conexao, "SELECT * FROM tb_jogo WHERE co_seq_jogo = @id");
        AdicionarParametro(comando, "@id", id);
        await using var leitor = await comando.ExecuteReaderAsync();

        return await leitor.ReadAsync() ? Mapear(leitor) : null;
    }

    public async Task<int> InserirAsync(Jogo jogo)
    {
        await using var conexao = await AbrirConexaoAsync();
        await using var comando = CriarComando(conexao,
            "INSERT INTO tb_jogo (no_jogo, ds_jogo, nu_ano_lancamento, vl_preco, qt_estoque, st_ativo, co_plataforma, co_condicao_jogo, dt_cadastro) " +
            "VALUES (@nome, @desc, @ano, @preco, @estoque, @ativo, @plataforma, @condicao, @data)");
        
        AdicionarParametrosJogo(comando, jogo);
        await ExecutarGravacaoAsync(comando);
        return (int)comando.LastInsertedId;
    }

    public async Task AlterarAsync(Jogo jogo)
    {
        await using var conexao = await AbrirConexaoAsync();
        await using var comando = CriarComando(conexao,
            "UPDATE tb_jogo SET no_jogo = @nome, ds_jogo = @desc, nu_ano_lancamento = @ano, vl_preco = @preco, " +
            "qt_estoque = @estoque, st_ativo = @ativo, co_plataforma = @plataforma, co_condicao_jogo = @condicao WHERE co_seq_jogo = @id");
        
        AdicionarParametrosJogo(comando, jogo);
        AdicionarParametro(comando, "@id", jogo.Id);
        await ExecutarGravacaoAsync(comando);
    }

    public async Task DesativarAsync(int id)
    {
        await using var conexao = await AbrirConexaoAsync();
        await using var comando = CriarComando(conexao, "UPDATE tb_jogo SET st_ativo = 0 WHERE co_seq_jogo = @id");
        AdicionarParametro(comando, "@id", id);
        await ExecutarGravacaoAsync(comando);
    }

    private void AdicionarParametros(MySqlCommand comando, string? nome, int? plataformaId)
    {
        if (!string.IsNullOrWhiteSpace(nome)) AdicionarParametro(comando, "@nome", $"%{nome}%");
        if (plataformaId.HasValue) AdicionarParametro(comando, "@plataformaId", plataformaId.Value);
    }

    private void AdicionarParametrosJogo(MySqlCommand comando, Jogo jogo)
    {
        AdicionarParametro(comando, "@nome", jogo.Nome);
        AdicionarParametro(comando, "@desc", jogo.Descricao ?? (object)DBNull.Value);
        AdicionarParametro(comando, "@ano", jogo.AnoDeLancamento);
        AdicionarParametro(comando, "@preco", jogo.Preco);
        AdicionarParametro(comando, "@estoque", jogo.Estoque);
        AdicionarParametro(comando, "@ativo", jogo.Ativo);
        AdicionarParametro(comando, "@plataforma", jogo.IdPlataforma); // Alterado aqui
        AdicionarParametro(comando, "@condicao", jogo.IdCondicaoJogo); // Alterado aqui
        AdicionarParametro(comando, "@data", jogo.DataCadastro);
    }

    private static Jogo Mapear(MySqlDataReader leitor) => new()
    {
        Id = leitor.GetInt32("co_seq_jogo"),
        Nome = leitor.GetString("no_jogo"),
        Descricao = leitor.IsDBNull(leitor.GetOrdinal("ds_jogo")) ? null : leitor.GetString("ds_jogo"),
        AnoDeLancamento = leitor.GetInt32("nu_ano_lancamento"),
        Preco = leitor.GetDecimal("vl_preco"),
        Estoque = leitor.GetInt32("qt_estoque"),
        Ativo = leitor.GetBoolean("st_ativo"),
        IdPlataforma = leitor.GetInt32("co_plataforma"),     // Alterado aqui
        IdCondicaoJogo = leitor.GetInt32("co_condicao_jogo"), // Alterado aqui
        DataCadastro = leitor.GetDateTime("dt_cadastro")
    };
}