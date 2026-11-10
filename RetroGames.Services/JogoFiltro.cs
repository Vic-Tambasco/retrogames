using System.Collections.Generic;

namespace RetroGames.Services;

public class JogoFiltro
{
    public string? Nome { get; set; }
    public int? PlataformaId { get; set; }
    public List<int>? GeneroIds { get; set; } 
    public int Pagina { get; set; } = 1;
    public int TamanhoPagina { get; set; } = 10;
    public bool IncluirInativos { get; set; } = false; 
}