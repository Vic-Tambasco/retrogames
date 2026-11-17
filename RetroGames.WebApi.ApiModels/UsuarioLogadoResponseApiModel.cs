namespace RetroGames.WebApi.ApiModels;

public class UsuarioLogadoResponseApiModel
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int IdPerfil { get; set; }
}