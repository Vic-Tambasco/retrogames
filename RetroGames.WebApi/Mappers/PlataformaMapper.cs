using RetroGames.Model.Model;
using RetroGames.WebApi.ApiModels;

namespace RetroGames.WebApi.Mappers;

public static class PlataformaMapper
{
    // ... (mantenha o ParaResponse que já existe) ...
    public static ListarPlataformaResponseApiModel ParaResponse(this Plataforma plataforma)
    {
        return new ListarPlataformaResponseApiModel
        {
            Id = plataforma.Id,
            Nome = plataforma.Nome,
            Fabricante = plataforma.Fabricante,
            AnoDeLancamento = plataforma.AnoDeLancamento
        };
    }

    // --- Novos métodos da Aula 5 ---
    public static Plataforma ParaModel(this InserirPlataformaRequestApiModel request)
    {
        return new Plataforma
        {
            Nome = request.Nome,
            Fabricante = request.Fabricante,
            AnoDeLancamento = request.AnoDeLancamento
        };
    }

    public static Plataforma ParaModel(this AlterarPlataformaRequestApiModel request, int id)
    {
        return new Plataforma
        {
            Id = id,
            Nome = request.Nome,
            Fabricante = request.Fabricante,
            AnoDeLancamento = request.AnoDeLancamento
        };
    }
}