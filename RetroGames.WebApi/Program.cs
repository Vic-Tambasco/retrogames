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

app.UseExceptionHandler();
using (var scope = app.Services.CreateScope())
{
    var authService = scope.ServiceProvider.GetRequiredService<IAutenticacaoService>();
    var adminEmail = builder.Configuration["AdminInicial:Email"];
    var adminSenha = builder.Configuration["AdminInicial:Senha"];
    
    if (!string.IsNullOrWhiteSpace(adminEmail) && !string.IsNullOrWhiteSpace(adminSenha))
    {
        await authService.GarantirAdministradorInicialAsync(adminEmail, adminSenha);
    }
}
app.UseAuthentication();
app.UseAuthorization();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "RetroGames API");
    });
}

app.MapControllers();

app.Run();