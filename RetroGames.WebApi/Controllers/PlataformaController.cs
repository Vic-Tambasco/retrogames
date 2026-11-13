using Microsoft.AspNetCore.Mvc;
using RetroGames.Services;
using RetroGames.WebApi.ApiModels;
using RetroGames.WebApi.Mappers;

namespace RetroGames.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlataformaController : ControllerBase
{
    private readonly IPlataformaService _plataformaService;

    public PlataformaController(IPlataformaService plataformaService)
    {
        _plataformaService = plataformaService;
    }

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
}