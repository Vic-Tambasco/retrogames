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

    // 106 - 409: E-mail já cadastrado (Desafio)
    public static readonly (int Code, string DefaultMessage, HttpStatusCode Status) EMAIL_JA_CADASTRADO = 
        (106, "Já existe uma conta com este e-mail.", HttpStatusCode.Conflict);
    // 109 - 404: Pedido não encontrado
    public static readonly (int Code, string DefaultMessage, HttpStatusCode Status) PEDIDO_NAO_ENCONTRADO =
        (109, "Pedido não encontrado.", HttpStatusCode.NotFound);

    // 110 - 400: Jogo indisponível (inativo ou não existe)
    public static readonly (int Code, string DefaultMessage, HttpStatusCode Status) JOGO_INDISPONIVEL =
        (110, "Um ou mais jogos não estão disponíveis para venda.", HttpStatusCode.BadRequest);

    // 111 - 409: Estoque insuficiente
    public static readonly (int Code, string DefaultMessage, HttpStatusCode Status) ESTOQUE_INSUFICIENTE =
        (111, "Estoque insuficiente.", HttpStatusCode.Conflict);

    // 112 - 400: Pedido sem itens
    public static readonly (int Code, string DefaultMessage, HttpStatusCode Status) PEDIDO_SEM_ITENS =
        (112, "O pedido deve conter ao menos um item.", HttpStatusCode.BadRequest);

    // 113 - 400: Quantidade inválida no pedido
    public static readonly (int Code, string DefaultMessage, HttpStatusCode Status) QUANTIDADE_PEDIDO_INVALIDA =
        (113, "A quantidade de cada item deve estar entre 1 e 10.", HttpStatusCode.BadRequest);
}