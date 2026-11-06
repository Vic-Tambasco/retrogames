
using RetroGames.Repository;
using RetroGames.Repository.Interfaces;

// ATENÇÃO: Troque SUA_SENHA pela senha do seu MySQL!
var connectionString = "server=localhost;port=3306;database=retrogames;user=root;password=SUA_SENHA";

var config = new RepositoryConfig(connectionString);
IRepositorySet repo = new RepositorySet(config);

Console.WriteLine("--- INICIANDO TESTES DO REPOSITORY ---\n");

try
{
    // TESTE 1: Listar Plataformas
    Console.WriteLine("1. Testando Plataformas...");
    var plataformas = await repo.Plataforma.ListarAsync();
    Console.WriteLine($"   Sucesso! Encontradas {plataformas.Count} plataformas.");
    if (plataformas.Any()) Console.WriteLine($"   Exemplo: {plataformas[0].Nome}");

    // TESTE 2: Listar Gêneros
    Console.WriteLine("\n2. Testando Gêneros...");
    var generos = await repo.Genero.ListarAsync();
    Console.WriteLine($"   Sucesso! Encontrados {generos.Count} gêneros.");
    if (generos.Any()) Console.WriteLine($"   Exemplo: {generos[0].Nome}");

    // TESTE 3: Listar Jogos (Paginado)
    Console.WriteLine("\n3. Testando Jogos (Página 1, 5 itens)...");
    var (jogos, total) = await repo.Jogo.ListarAsync(nome: null, plataformaId: null, generoIds: null, incluirInativos: false, pagina: 1, tamanhoPagina: 5);
    Console.WriteLine($"   Sucesso! Total de jogos no banco: {total}. Retornados nesta página: {jogos.Count}.");
    if (jogos.Any()) Console.WriteLine($"   Exemplo: {jogos[0].Nome} (Preço: {jogos[0].Preco})");

    // TESTE 4: Buscar um Jogo por ID
    if (jogos.Any())
    {
        var idTeste = jogos[0].Id;
        Console.WriteLine($"\n4. Testando Busca por ID (Buscando ID {idTeste})...");
        var jogoBuscado = await repo.Jogo.BuscarPorIdAsync(idTeste);
        Console.WriteLine($"   Sucesso! Jogo encontrado: {jogoBuscado?.Nome}");
    }

    // TESTE 5: Buscar Condições de Jogo
    Console.WriteLine("\n5. Testando Condições de Jogo...");
    var condicoes = await repo.CondicaoJogo.ListarAsync();
    Console.WriteLine($"   Sucesso! Encontradas {condicoes.Count} condições.");
    if (condicoes.Any()) Console.WriteLine($"   Exemplo: {condicoes[0].Descricao}");

    Console.WriteLine("\n--- TODOS OS TESTES PASSARAM! ---");
}
catch (Exception ex)
{
    Console.WriteLine($"\n[ERRO FATAL] Falha ao conectar ou executar consulta:\n{ex.Message}");
}