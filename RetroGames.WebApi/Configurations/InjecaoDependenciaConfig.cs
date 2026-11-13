using Microsoft.Extensions.DependencyInjection;
using RetroGames.Repository;
using RetroGames.Repository.Interfaces;
using RetroGames.Services;

namespace RetroGames.WebApi.Configuration;

public static class InjecaoDependenciaConfig
{
    public static IServiceCollection AddRepositorios(this IServiceCollection services, string connectionString)
    {
        // Seu RepositoryConfig recebe a string no construtor
        services.AddSingleton(new RepositoryConfig(connectionString));

        // Seu RepositorySet já instancia todos os repositórios por dentro
        services.AddScoped<IRepositorySet, RepositorySet>();

        return services;
    }

    public static IServiceCollection AddServicosDeNegocio(this IServiceCollection services)
    {
        // ATENÇÃO: Se as interfaces de Service estiverem em outra pasta, 
        // pode ser necessário adicionar um "using RetroGames.Services.Interfaces;" no topo.
        services.AddScoped<IPlataformaService, PlataformaService>();
        services.AddScoped<IGeneroService, GeneroService>();
        services.AddScoped<ICondicaoJogoService, CondicaoJogoService>();
        services.AddScoped<IJogoService, JogoService>();

        return services;
    }
}