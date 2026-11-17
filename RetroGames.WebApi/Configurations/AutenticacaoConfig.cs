using Microsoft.AspNetCore.Authentication.Cookies;

namespace RetroGames.WebApi.Configuration;

public static class AutenticacaoConfig
{
    public static void AddConfiguracaoAutenticacao(this IServiceCollection services)
    {
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.Cookie.Name = "retrogames.sessao";
                options.Cookie.HttpOnly = true;
                options.Cookie.SameSite = SameSiteMode.Strict;
                options.ExpireTimeSpan = TimeSpan.FromHours(2);
                options.SlidingExpiration = true;

                // Como é uma API, não redirecionamos para uma página de login
                options.Events.OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return context.Response.WriteAsJsonAsync(new RetroGamesResponseApiModel(
                        ResponseCodes.NAO_AUTENTICADO.Code, ResponseCodes.NAO_AUTENTICADO.DefaultMessage));
                };
                options.Events.OnRedirectToAccessDenied = context =>
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    return context.Response.WriteAsJsonAsync(new RetroGamesResponseApiModel(
                        ResponseCodes.ACESSO_NEGADO.Code, ResponseCodes.ACESSO_NEGADO.DefaultMessage));
                };
            });
    }
}