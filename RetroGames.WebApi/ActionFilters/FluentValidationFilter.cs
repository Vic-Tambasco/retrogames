using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RetroGames.WebApi.ApiModels;

namespace RetroGames.WebApi.ActionFilters;

public class FluentValidationFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        // 1. Proteção contra modelo nulo
        foreach (var argument in context.ActionArguments.Values)
        {
            if (argument == null)
            {
                var envelope = new RetroGamesResponseApiModel(ResponseCodes.MODEL_NULL.Code, ResponseCodes.MODEL_NULL.DefaultMessage);
                context.Result = new BadRequestObjectResult(envelope);
                return;
            }
        }

        // 2. Executa os validators registrados
        foreach (var argument in context.ActionArguments.Values)
        {
            if (argument == null) continue;

            var argumentType = argument.GetType();
            var validatorType = typeof(IValidator<>).MakeGenericType(argumentType);
            var validator = context.HttpContext.RequestServices.GetService(validatorType) as IValidator;

            if (validator != null)
            {
                var validationContext = new ValidationContext<object>(argument);
                var validationResult = await validator.ValidateAsync(validationContext);

                if (!validationResult.IsValid)
                {
                    var erros = validationResult.Errors
                        .GroupBy(e => char.ToLowerInvariant(e.PropertyName[0]) + e.PropertyName.Substring(1))
                        .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());

                    var envelope = new RetroGamesResponseApiModel<Dictionary<string, string[]>>(
                        ResponseCodes.MODEL_STATE_INVALIDO.Code, 
                        ResponseCodes.MODEL_STATE_INVALIDO.DefaultMessage) 
                    { 
                        Data = erros 
                    };
                    
                    context.Result = new BadRequestObjectResult(envelope);
                    return;
                }
            }
        }

        await next();
    }
}