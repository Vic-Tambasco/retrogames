using Microsoft.AspNetCore.Authorization;

namespace RetroGames.WebApi.Configurations;

public static class AutorizacaoConfig
{
    public static void AddConfiguracaoAutorizacao(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            // Política padrão: exige usuário logado (aplica em tudo que não tiver [AllowAnonymous])
            options.FallbackPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();

            // Política de Administrador: exige o perfil de Administrador
            options.AddPolicy("SomenteAdministrador", policy =>
                policy.RequireRole("Administrador"));
        });
    }
}