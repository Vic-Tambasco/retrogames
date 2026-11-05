using MySqlConnector;

namespace RetroGames.Repository;

public abstract class RepositoryBase
{
    private readonly RepositoryConfig _config;

    protected RepositoryBase(RepositoryConfig config)
    {
        _config = config;
    }

    // Abre uma nova conexão com o MySQL
    protected async Task<MySqlConnection> AbrirConexaoAsync()
    {
        var conexao = new MySqlConnection(_config.ConnectionString);
        await conexao.OpenAsync();
        return conexao;
    }

    // Cria um comando com o texto SQL e a conexão
    protected MySqlCommand CriarComando(MySqlConnection conexao, string sql)
    {
        return new MySqlCommand(sql, conexao);
    }

    // Adiciona um parâmetro ao comando (evita SQL Injection)
    protected void AdicionarParametro(MySqlCommand comando, string nome, object valor)
    {
        comando.Parameters.AddWithValue(nome, valor);
    }

    // Executa uma gravação (INSERT/UPDATE) e trata o erro 1062
    protected async Task<int> ExecutarGravacaoAsync(MySqlCommand comando)
    {
        try
        {
            return await comando.ExecuteNonQueryAsync();
        }
        catch (MySqlException ex) when (ex.Number == 1062)
        {
            // Erro 1062 = Duplicate entry for key
            throw new RegistroDuplicadoException("Já existe um registro com este valor único.", ex);
        }
    }
}