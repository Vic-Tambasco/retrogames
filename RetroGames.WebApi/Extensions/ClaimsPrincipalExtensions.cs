using System.Security.Claims;
using RetroGames.Model.Constantes;
using RetroGames.Model.Model;

namespace RetroGames.WebApi.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static ClaimsPrincipal ParaClaimsPrincipal(this Usuario usuario)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new Claim(ClaimTypes.Name, usuario.Nome),
            new Claim(ClaimTypes.Email, usuario.Email)
        };

        // Usa a constante Perfis.Administrador
        if (usuario.IdPerfil == Perfis.Administrador) 
        {
            claims.Add(new Claim(ClaimTypes.Role, "Administrador"));
        }

        var identity = new ClaimsIdentity(claims, "CookieAuth");
        return new ClaimsPrincipal(identity);
    }

    public static int ObterUsuarioId(this ClaimsPrincipal user)
    {
        var idClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.TryParse(idClaim, out var id) ? id : 0;
    }
}