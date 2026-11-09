using System.Net;

namespace RetroGames.Services;

public static class BusinessErrors
{
    // 100 - 404: Plataforma pedida ou do jogo não existe (RN05)
    public static readonly (int Code, string DefaultMessage, HttpStatusCode Status) PLATAFORMA_NAO_ENCONTRADA = 
        (100, "Plataforma não encontrada.", HttpStatusCode.NotFound);

    // 101 - 409: Nome já usado (RN01)
    public static readonly (int Code, string DefaultMessage, HttpStatusCode Status) PLATAFORMA_NOME_DUPLICADO = 
        (101, "Já existe uma plataforma com este nome.", HttpStatusCode.Conflict);

    // 102 - 409: Plataforma com jogos (RN02)
    public static readonly (int Code, string DefaultMessage, HttpStatusCode Status) PLATAFORMA_POSSUI_JOGOS = 
        (102, "A plataforma não pode ser excluída pois possui jogos associados.", HttpStatusCode.Conflict);

    // 103 - 404: O id do jogo não existe
    public static readonly (int Code, string DefaultMessage, HttpStatusCode Status) JOGO_NAO_ENCONTRADO = 
        (103, "Jogo não encontrado.", HttpStatusCode.NotFound);

    // 104 - 404: Condição do jogo não existe (RN05)
    public static readonly (int Code, string DefaultMessage, HttpStatusCode Status) CONDICAO_NAO_ENCONTRADA = 
        (104, "Condição de jogo não encontrada.", HttpStatusCode.NotFound);

    // 105 - 404: Gênero do jogo não existe (RN05)
    public static readonly (int Code, string DefaultMessage, HttpStatusCode Status) GENERO_NAO_ENCONTRADO = 
        (105, "Gênero não encontrado.", HttpStatusCode.NotFound);
}