using Microsoft.AspNetCore.Mvc;
using RetroGames.Services;
using RetroGames.WebApi.ApiModels;
using RetroGames.WebApi.Mappers;

namespace RetroGames.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class JogoController : ControllerBase
{
    private readonly IJogoService _jogoService;

    public JogoController(IJogoService jogoService)
    {
        _jogoService = jogoService;
    }

    [HttpGet]
    public async Task<ActionResult<RetroGamesResponseApiModel<PaginaResponseApiModel<ListarJogoResponseApiModel>>>> Listar([FromQuery] ListarJogoRequestApiModel request)
    {
        var filtro = request.ParaFiltro();
        var (itens, total) = await _jogoService.ListarAsync(filtro);
        
        var totalPaginas = (int)Math.Ceiling((double)total / filtro.TamanhoPagina);

        var response = new PaginaResponseApiModel<ListarJogoResponseApiModel>
        {
            Itens = itens.Select(j => j.ParaListarResponse()).ToList(),
            Pagina = filtro.Pagina,
            TamanhoPagina = filtro.TamanhoPagina,
            TotalItens = total,
            TotalPaginas = totalPaginas
        };

        return Ok(new RetroGamesResponseApiModel<PaginaResponseApiModel<ListarJogoResponseApiModel>>(response));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<RetroGamesResponseApiModel<ListarPorIdJogoResponseApiModel>>> ListarPorId(int id)
    {
        var jogo = await _jogoService.BuscarPorIdAsync(id);
        
        return Ok(new RetroGamesResponseApiModel<ListarPorIdJogoResponseApiModel>(jogo.ParaDetalheResponse()));
    }
}