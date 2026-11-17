using Microsoft.AspNetCore.Mvc;
using RetroGames.Services;
using RetroGames.WebApi.ApiModels;
using RetroGames.WebApi.Mappers;
using Microsoft.AspNetCore.Authorization;

namespace RetroGames.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class GeneroController : ControllerBase
{
    private readonly IGeneroService _generoService;

    public GeneroController(IGeneroService generoService)
    {
        _generoService = generoService;
    }

    [HttpGet]
    public async Task<ActionResult<RetroGamesResponseApiModel<IEnumerable<ListarGeneroResponseApiModel>>>> Listar()
    {
        var generos = await _generoService.ListarAsync();
        var response = generos.Select(g => g.ParaResponse()).ToList();
        
        return Ok(new RetroGamesResponseApiModel<IEnumerable<ListarGeneroResponseApiModel>>(response));
    }
}