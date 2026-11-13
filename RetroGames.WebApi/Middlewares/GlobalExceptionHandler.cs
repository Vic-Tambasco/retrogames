using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using RetroGames.Services;
using RetroGames.WebApi.ApiModels;

namespace RetroGames.WebApi.Middlewares;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is BusinessRuleException businessRuleException)
        {
            // Erro de negócio: usa o status e o código do catálogo BusinessErrors
            var erro = businessRuleException.Erro;
            
            httpContext.Response.StatusCode = (int)erro.Status;
            httpContext.Response.ContentType = "application/json";
            
            var response = new RetroGamesResponseApiModel(erro.Code, exception.Message);
            await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);
            
            return true;
        }

        // Qualquer outra exceção: 500 genérico e loga o erro completo
        _logger.LogError(exception, "Ocorreu um erro inesperado.");
        
        httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        httpContext.Response.ContentType = "application/json";
        
        var erroGenerico = new RetroGamesResponseApiModel(ResponseCodes.ERRO.Code, ResponseCodes.ERRO.DefaultMessage);
        await httpContext.Response.WriteAsJsonAsync(erroGenerico, cancellationToken);

        return true;
    }
}