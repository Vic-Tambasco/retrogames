using DotNetEnv;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using RetroGames.WebApi.ActionFilters;
using RetroGames.WebApi.ApiModels;
using RetroGames.WebApi.Configurations;
using RetroGames.WebApi.Middlewares;
using RetroGames.Services;

Env.TraversePath().Load();

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Default");
if (string.IsNullOrWhiteSpace(connectionString))
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("ERRO FATAL: A connection string 'ConnectionStrings__Default' não foi encontrada no arquivo .env.");
    Console.ResetColor();
    return;
}

builder.Services.AddRepositorios(connectionString);
builder.Services.AddServicosDeNegocio();

builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.AddControllers(options =>
{
    options.Filters.Add<FluentValidationFilter>();
}).ConfigureApiBehaviorOptions(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        return new BadRequestObjectResult(new RetroGamesResponseApiModel(
            ResponseCodes.MODEL_STATE_INVALIDO.Code,
            ResponseCodes.MODEL_STATE_INVALIDO.DefaultMessage));
    };
});

builder.Services.AddOpenApi();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddScoped<Microsoft.AspNetCore.Identity.IPasswordHasher<RetroGames.Model.Model.Usuario>, Microsoft.AspNetCore.Identity.PasswordHasher<RetroGames.Model.Model.Usuario>>();
builder.Services.AddScoped<IAutenticacaoService, AutenticacaoService>();
builder.Services.AddConfiguracaoAutenticacao();
builder.Services.AddHttpContextAccessor(); 

builder.Services.AddConfiguracaoAutorizacao();
builder.Services.AddScoped<IUsuarioAtualService, RetroGames.WebApi.Services.UsuarioAtualService>();

var app = builder.Build();

using (var escopo = app.Services.CreateScope())
{
    var emailAdmin = builder.Configuration["AdminInicial:Email"];
    var senhaAdmin = builder.Configuration["AdminInicial:Senha"];

    if (!string.IsNullOrWhiteSpace(emailAdmin) && !string.IsNullOrWhiteSpace(senhaAdmin))
    {
        var autenticacao = escopo.ServiceProvider.GetRequiredService<IAutenticacaoService>();
        await autenticacao.GarantirAdministradorInicialAsync(emailAdmin, senhaAdmin);
    }
    else
    {
        app.Logger.LogWarning("AdminInicial__Email/AdminInicial__Senha não configurados no .env.");
    }
}

app.UseExceptionHandler();

// O SWAGGER PRECISA VIR ANTES DO USE AUTHENTICATION E USE AUTHORIZATION
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi().AllowAnonymous(); // Libera o JSON
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "RetroGames API");
    });
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers(); // Mapeia os controllers depois da segurança

app.Run();