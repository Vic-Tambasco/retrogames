using Microsoft.AspNetCore.Mvc;
using RetroGames.Services;
using RetroGames.WebApi.ApiModels;
using RetroGames.WebApi.Mappers;
using Microsoft.AspNetCore.Authorization;
using RetroGames.WebApi.Configurations;

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

    [AllowAnonymous]
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
    [AllowAnonymous]
    [HttpGet("{id:int}")]
    public async Task<ActionResult<RetroGamesResponseApiModel<ListarPorIdJogoResponseApiModel>>> ListarPorId(int id)
    {
        var jogo = await _jogoService.BuscarPorIdAsync(id);
        
        return Ok(new RetroGamesResponseApiModel<ListarPorIdJogoResponseApiModel>(jogo.ParaDetalheResponse()));
    }

    [Authorize(Policy = Politicas.Administrador)]
    [HttpPost]
    public async Task<ActionResult<RetroGamesResponseApiModel<ListarPorIdJogoResponseApiModel>>> Inserir(InserirJogoRequestApiModel request)
    {
        var jogo = await _jogoService.InserirAsync(request.ParaModel(), request.IdsGeneros);
        
        // O Service altera o ID no objeto 'jogo', mas para devolver os detalhes completos, buscamos ele de novo
        var detalhe = await _jogoService.BuscarPorIdAsync(jogo.Id);

        return CreatedAtAction(
            nameof(ListarPorId), 
            new { id = detalhe.Jogo.Id }, 
            new RetroGamesResponseApiModel<ListarPorIdJogoResponseApiModel>(detalhe.ParaDetalheResponse())
        );
    }
    [Authorize(Policy = Politicas.Administrador)]
    [HttpPut("{id:int}")]
    public async Task<ActionResult<RetroGamesResponseApiModel<ListarPorIdJogoResponseApiModel>>> Alterar(int id, AlterarJogoRequestApiModel request)
    {
        var jogo = await _jogoService.AlterarAsync(request.ParaModel(id), request.IdsGeneros);
        
        var detalhe = await _jogoService.BuscarPorIdAsync(jogo.Id);

        return Ok(new RetroGamesResponseApiModel<ListarPorIdJogoResponseApiModel>(detalhe.ParaDetalheResponse()));
    }
    [Authorize(Policy = Politicas.Administrador)]
    [HttpDelete("{id:int}")]
    public async Task<ActionResult<RetroGamesResponseApiModel<object>>> Excluir(int id)
    {
        await _jogoService.DesativarAsync(id);
        
        return Ok(new RetroGamesResponseApiModel<object>(new { id }));
    }
}