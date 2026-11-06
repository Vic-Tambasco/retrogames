using MySqlConnector;
using RetroGames.Model.Model;
using RetroGames.Repository.Interfaces;

namespace RetroGames.Repository;

public class GeneroRepository : RepositoryBase, IGeneroRepository
{
    public GeneroRepository(RepositoryConfig config) : base(config) { }

    public async Task<List<Genero>> ListarAsync()
    {
        var lista = new List<Genero>();
        await using var conexao = await AbrirConexaoAsync();
        await using var comando = CriarComando(conexao, "SELECT co_seq_genero, no_genero FROM tb_genero ORDER BY no_genero");
        await using var leitor = await comando.ExecuteReaderAsync();

        while (await leitor.ReadAsync())
        {
            lista.Add(new Genero 
            { 
                Id = leitor.GetInt32("co_seq_genero"), 
                Nome = leitor.GetString("no_genero") 
            });
        }

        return lista;
    }
}