using Microsoft.AspNetCore.Authorization;
using RetroGames.Model.Constantes;

namespace RetroGames.WebApi.Configurations;

public static class AutorizacaoConfig
{
    public static void AddConfiguracaoAutorizacao(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.FallbackPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();

            options.AddPolicy(Politicas.Administrador, policy =>
                policy.RequireRole(Perfis.Administrador));
        });
    }
}