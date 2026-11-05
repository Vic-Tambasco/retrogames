namespace RetroGames.Model.Model;

public class Usuario
{
    // Coluna: co_seq_usuario
    public int Id { get; set; }

    // Coluna: no_usuario
    public string Nome { get; set; } = string.Empty;

    // Coluna: ds_email
    public string Email { get; set; } = string.Empty;

    // Coluna: ds_senha_hash
    public string SenhaHash { get; set; } = string.Empty;

    // Coluna: co_perfil
    public int IdPerfil { get; set; }
}