namespace RetroGames.WebApi.ApiModels;

public class AlterarPlataformaRequestApiModel
{
    public string Nome { get; set; } = string.Empty;
    public string Fabricante { get; set; } = string.Empty;
    public int AnoDeLancamento { get; set; }
}