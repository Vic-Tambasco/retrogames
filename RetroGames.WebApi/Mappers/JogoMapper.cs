using RetroGames.Services;
using RetroGames.WebApi.ApiModels;

namespace RetroGames.WebApi.Mappers;

public static class JogoMapper
{
    public static JogoFiltro ParaFiltro(this ListarJogoRequestApiModel request)
    {
        return new JogoFiltro
        {
            Nome = request.Busca,
            PlataformaId = request.IdPlataforma,
            // Se o seu filtro usar uma lista de generos, passamos uma lista com o id (se existir)
            GeneroIds = request.IdGenero.HasValue ? new List<int> { request.IdGenero.Value } : new List<int>(),
            IncluirInativos = false, // Regra da Aula 4: listagem só mostra ativos
            Pagina = request.Pagina,
            TamanhoPagina = request.TamanhoPagina
        };
    }

    public static ListarJogoResponseApiModel ParaListarResponse(this JogoDetalheServiceModel detalhe)
    {
        return new ListarJogoResponseApiModel
        {
            Id = detalhe.Jogo.Id,
            Nome = detalhe.Jogo.Nome,
            AnoDeLancamento = detalhe.Jogo.AnoDeLancamento,
            Preco = detalhe.Jogo.Preco,
            Estoque = detalhe.Jogo.Estoque,
            Ativo = detalhe.Jogo.Ativo,
            NomePlataforma = detalhe.NomePlataforma,
            DescricaoCondicao = detalhe.DescricaoCondicao,
            NomesGeneros = detalhe.Generos.Select(g => g.Nome).ToList()
        };
    }

    public static ListarPorIdJogoResponseApiModel ParaDetalheResponse(this JogoDetalheServiceModel detalhe)
    {
        return new ListarPorIdJogoResponseApiModel
        {
            Id = detalhe.Jogo.Id,
            Nome = detalhe.Jogo.Nome,
            AnoDeLancamento = detalhe.Jogo.AnoDeLancamento,
            Preco = detalhe.Jogo.Preco,
            Estoque = detalhe.Jogo.Estoque,
            Ativo = detalhe.Jogo.Ativo,
            Descricao = detalhe.Jogo.Descricao,
            IdPlataforma = detalhe.Jogo.IdPlataforma,
            NomePlataforma = detalhe.NomePlataforma,
            IdCondicaoJogo = detalhe.Jogo.IdCondicaoJogo,
            DescricaoCondicao = detalhe.DescricaoCondicao,
            Generos = detalhe.Generos.Select(g => new GeneroJogoApiModel { Id = g.Id, Nome = g.Nome }).ToList()
        };
    }
}