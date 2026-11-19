using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RetroGames.Services;
using RetroGames.WebApi.ApiModels;
using RetroGames.WebApi.Extensions;

namespace RetroGames.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AutenticacaoController : ControllerBase
{
    private readonly IAutenticacaoService _autenticacaoService;

    public AutenticacaoController(IAutenticacaoService autenticacaoService)
    {
        _autenticacaoService = autenticacaoService;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<RetroGamesResponseApiModel<UsuarioLogadoResponseApiModel>>> Login(LoginRequestApiModel request)
    {
        var usuario = await _autenticacaoService.AutenticarAsync(request.Email, request.Senha);

        if (usuario == null)
        {
            return Unauthorized(new RetroGamesResponseApiModel(
                ResponseCodes.CREDENCIAIS_INVALIDAS.Code, 
                ResponseCodes.CREDENCIAIS_INVALIDAS.DefaultMessage));
        }

        var claimsPrincipal = usuario.ParaClaimsPrincipal();
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

        var response = new UsuarioLogadoResponseApiModel
        {
            Id = usuario.Id,
            Nome = usuario.Nome,
            Email = usuario.Email,
            IdPerfil = usuario.IdPerfil
        };

        return Ok(new RetroGamesResponseApiModel<UsuarioLogadoResponseApiModel>(response));
    }

    [HttpPost("logout")]
    public async Task<ActionResult<RetroGamesResponseApiModel>> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Ok(new RetroGamesResponseApiModel(ResponseCodes.SUCESSO.Code));
    }

    [HttpGet("eu")]
    public ActionResult<RetroGamesResponseApiModel<UsuarioLogadoResponseApiModel>> Eu()
    {
        var usuarioId = User.ObterUsuarioId();
        var nome = User.Identity?.Name;
        var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
        
        
        var response = new UsuarioLogadoResponseApiModel
        {
            Id = usuarioId,
            Nome = nome ?? string.Empty,
            Email = email ?? string.Empty,
            IdPerfil = User.ObterPerfilId()
        };

        return Ok(new RetroGamesResponseApiModel<UsuarioLogadoResponseApiModel>(response));
    }
}