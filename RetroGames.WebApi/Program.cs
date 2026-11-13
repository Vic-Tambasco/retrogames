using DotNetEnv;
using RetroGames.WebApi.Configuration;

// 1. Carrega o arquivo .env da raiz do repositório
Env.TraversePath().Load();

var builder = WebApplication.CreateBuilder(args);

// 2. Lê a connection string
var connectionString = builder.Configuration.GetConnectionString("Default");
if (string.IsNullOrWhiteSpace(connectionString))
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("ERRO FATAL: A connection string 'ConnectionStrings__Default' não foi encontrada no arquivo .env.");
    Console.ResetColor();
    return;
}

// 3. Registra as dependências
builder.Services.AddRepositorios(connectionString);
builder.Services.AddServicosDeNegocio();
builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

// 4. Pipeline HTTP
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