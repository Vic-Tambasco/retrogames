using Microsoft.AspNetCore.Mvc;
using RetroGames.Services;
using RetroGames.WebApi.ApiModels;
using RetroGames.WebApi.Mappers;
using Microsoft.AspNetCore.Authorization;

namespace RetroGames.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class PlataformaController : ControllerBase
{
    private readonly IPlataformaService _plataformaService;

    public PlataformaController(IPlataformaService plataformaService)
    {
        _plataformaService = plataformaService;
    }

    // ... (mantenha as actions GET Listar e ListarPorId que já existem) ...
    [HttpGet]
    public async Task<ActionResult<RetroGamesResponseApiModel<IEnumerable<ListarPlataformaResponseApiModel>>>> Listar()
    {
        var plataformas = await _plataformaService.ListarAsync();
        var response = plataformas.Select(p => p.ParaResponse()).ToList();
        return Ok(new RetroGamesResponseApiModel<IEnumerable<ListarPlataformaResponseApiModel>>(response));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<RetroGamesResponseApiModel<ListarPlataformaResponseApiModel>>> ListarPorId(int id)
    {
        var plataforma = await _plataformaService.BuscarPorIdAsync(id);
        return Ok(new RetroGamesResponseApiModel<ListarPlataformaResponseApiModel>(plataforma.ParaResponse()));
    }

    // --- Novas actions da Aula 5 ---
    [HttpPost]
    public async Task<ActionResult<RetroGamesResponseApiModel<ListarPlataformaResponseApiModel>>> Inserir(InserirPlataformaRequestApiModel request)
    {
        var criada = await _plataformaService.InserirAsync(request.ParaModel());
        
        return CreatedAtAction(
            nameof(ListarPorId), 
            new { id = criada.Id }, 
            new RetroGamesResponseApiModel<ListarPlataformaResponseApiModel>(criada.ParaResponse())
        );
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<RetroGamesResponseApiModel<ListarPlataformaResponseApiModel>>> Alterar(int id, AlterarPlataformaRequestApiModel request)
    {
        var alterada = await _plataformaService.AlterarAsync(request.ParaModel(id));
        
        return Ok(new RetroGamesResponseApiModel<ListarPlataformaResponseApiModel>(alterada.ParaResponse()));
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<RetroGamesResponseApiModel<object>>> Excluir(int id)
    {
        await _plataformaService.ExcluirAsync(id);
        
        return Ok(new RetroGamesResponseApiModel<object>(new { id }));
    }
}