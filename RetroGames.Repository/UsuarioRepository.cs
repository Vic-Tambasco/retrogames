using MySqlConnector;
using RetroGames.Model.Model;
using RetroGames.Repository.Interfaces;

namespace RetroGames.Repository;

public class UsuarioRepository : RepositoryBase, IUsuarioRepository
{
    public UsuarioRepository(RepositoryConfig config) : base(config) { }

    public async Task<Usuario?> BuscarPorEmailAsync(string email)
    {
        using var conexao = await AbrirConexaoAsync();
        var sql = "SELECT co_seq_usuario, no_usuario, ds_email, ds_senha_hash, co_perfil FROM tb_usuario WHERE ds_email = @email";
        using var comando = new MySqlCommand(sql, conexao);
        comando.Parameters.AddWithValue("@email", email);
        
        using var reader = await comando.ExecuteReaderAsync();
        if (await reader.ReadAsync())
            return Mapear(reader);
        
        return null;
    }

    public async Task<Usuario?> BuscarPorIdAsync(int id)
    {
        using var conexao = await AbrirConexaoAsync();
        var sql = "SELECT co_seq_usuario, no_usuario, ds_email, ds_senha_hash, co_perfil FROM tb_usuario WHERE co_seq_usuario = @id";
        using var comando = new MySqlCommand(sql, conexao);
        comando.Parameters.AddWithValue("@id", id);
        
        using var reader = await comando.ExecuteReaderAsync();
        if (await reader.ReadAsync())
            return Mapear(reader);
        
        return null;
    }

    public async Task<bool> ExisteAdministradorAsync()
    {
        using var conexao = await AbrirConexaoAsync();
        // Assumindo que o IdPerfil 1 é Administrador
        var sql = "SELECT COUNT(1) FROM tb_usuario WHERE co_perfil = 1"; 
        using var comando = new MySqlCommand(sql, conexao);
        
        var count = Convert.ToInt32(await comando.ExecuteScalarAsync());
        return count > 0;
    }

    public async Task<int> InserirAsync(Usuario usuario)
    {
        using var conexao = await AbrirConexaoAsync();
        var sql = @"INSERT INTO tb_usuario (no_usuario, ds_email, ds_senha_hash, co_perfil) 
                    VALUES (@nome, @email, @senhaHash, @perfil);
                    SELECT LAST_INSERT_ID();";
        using var comando = new MySqlCommand(sql, conexao);
        comando.Parameters.AddWithValue("@nome", usuario.Nome);
        comando.Parameters.AddWithValue("@email", usuario.Email);
        comando.Parameters.AddWithValue("@senhaHash", usuario.SenhaHash);
        comando.Parameters.AddWithValue("@perfil", usuario.IdPerfil);
        
        return Convert.ToInt32(await comando.ExecuteScalarAsync());
    }

    private static Usuario Mapear(MySqlDataReader reader)
    {
        return new Usuario
        {
            Id = reader.GetInt32("co_seq_usuario"),
            Nome = reader.GetString("no_usuario"),
            Email = reader.GetString("ds_email"),
            SenhaHash = reader.GetString("ds_senha_hash"),
            IdPerfil = reader.GetInt32("co_perfil")
        };
    }
}