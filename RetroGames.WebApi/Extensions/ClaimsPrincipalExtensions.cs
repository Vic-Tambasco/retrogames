using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using RetroGames.Model.Constantes;
using RetroGames.Model.Model;

namespace RetroGames.WebApi.Extensions;

public static class ClaimsPrincipalExtensions
{
    public const string ClaimIdPerfil = "retrogames/id-perfil";

    public static ClaimsPrincipal ParaClaimsPrincipal(this Usuario usuario)
    {
        var nomePerfil = usuario.IdPerfil == PerfilIds.Administrador
            ? Perfis.Administrador
            : Perfis.Cliente;

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new(ClaimTypes.Name, usuario.Nome),
            new(ClaimTypes.Email, usuario.Email),
            new(ClaimTypes.Role, nomePerfil),
            new(ClaimIdPerfil, usuario.IdPerfil.ToString())
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        return new ClaimsPrincipal(identity);
    }

    public static int ObterUsuarioId(this ClaimsPrincipal user)
    {
        var idClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.TryParse(idClaim, out var id) ? id : 0;
    }

    public static int ObterPerfilId(this ClaimsPrincipal user)
    {
        var claim = user.FindFirst(ClaimIdPerfil)?.Value;
        return int.TryParse(claim, out var id) ? id : 0;
    }
}