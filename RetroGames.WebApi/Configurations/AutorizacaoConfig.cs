using Microsoft.AspNetCore.Authorization;

namespace RetroGames.WebApi.Configurations;

public static class AutorizacaoConfig
{
    public static void AddConfiguracaoAutorizacao(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            // A política padrão exige login em tudo...
            options.FallbackPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();

            // Política de Administrador
            options.AddPolicy("SomenteAdministrador", policy =>
                policy.RequireRole("Administrador"));
        });
    }
}