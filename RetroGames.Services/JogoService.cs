using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using RetroGames.Model.Model;
using RetroGames.Repository.Interfaces;

namespace RetroGames.Services;

public class JogoService : IJogoService
{
    private readonly IRepositorySet _repository;

    public JogoService(IRepositorySet repository)
    {
        _repository = repository;
    }

    public async Task<(IEnumerable<JogoDetalheServiceModel> Items, int Total)> ListarAsync(JogoFiltro filtro)
    {
        // Passando os 6 parâmetros exatos do seu IJogoRepository
        var resultado = await _repository.Jogo.ListarAsync(
            filtro.Nome, 
            filtro.PlataformaId, 
            filtro.GeneroIds, 
            filtro.IncluirInativos,
            filtro.Pagina, 
            filtro.TamanhoPagina
        );
        
        var detalhes = await MontarDetalhesAsync(resultado.Jogos);
        return (detalhes, resultado.Total);
    }

    public async Task<JogoDetalheServiceModel> BuscarPorIdAsync(int id)
    {
        var jogo = await _repository.Jogo.BuscarPorIdAsync(id);
        if (jogo == null)
        {
            throw new BusinessRuleException(BusinessErrors.JOGO_NAO_ENCONTRADO);
        }

        var detalhes = await MontarDetalhesAsync(new[] { jogo });
        return detalhes.First();
    }

    public async Task<Jogo> InserirAsync(Jogo jogo, IEnumerable<int> idsGeneros)
    {
        jogo.Nome = jogo.Nome.Trim();
        jogo.Ativo = true; 

        await ValidarReferenciasAsync(jogo, idsGeneros);

        var transactionOptions = new TransactionOptions
        {
            IsolationLevel = IsolationLevel.ReadCommitted,
            Timeout = TransactionManager.DefaultTimeout,
        };

        using (var transactionScope = new TransactionScope(TransactionScopeOption.Required,
                   transactionOptions, TransactionScopeAsyncFlowOption.Enabled))
        {
            jogo.Id = await _repository.Jogo.InserirAsync(jogo);

            // Usando a assinatura que o seu Repository possui
            if (idsGeneros.Any())
            {
                await _repository.JogoGenero.InserirAsync(jogo.Id, idsGeneros.ToList());
            }

            transactionScope.Complete(); 
        }

        return jogo;
    }

    public async Task<Jogo> AlterarAsync(Jogo jogo, IEnumerable<int> idsGeneros)
    {
        var jogoExistente = await _repository.Jogo.BuscarPorIdAsync(jogo.Id);
        if (jogoExistente == null)
        {
            throw new BusinessRuleException(BusinessErrors.JOGO_NAO_ENCONTRADO);
        }

        jogo.Nome = jogo.Nome.Trim();
        await ValidarReferenciasAsync(jogo, idsGeneros);

        var transactionOptions = new TransactionOptions
        {
            IsolationLevel = IsolationLevel.ReadCommitted,
            Timeout = TransactionManager.DefaultTimeout,
        };

        using (var transactionScope = new TransactionScope(TransactionScopeOption.Required,
                   transactionOptions, TransactionScopeAsyncFlowOption.Enabled))
        {
            await _repository.Jogo.AlterarAsync(jogo);

            // Como não temos o método ExcluirPorJogoAsync, precisamos ver como seu repositório limpa.
            // Se você tiver um método ExcluirPorJogoAsync, use-o. 
            // Caso contrário, essa chamada abaixo pode falhar. Vou assumir que você tem um método ExcluirAsync(int idJogo)
            // Se o seu JogoGeneroRepository tiver ExcluirAsync(int idJogo, List<int> generos), use ele.
            
            // NOTA: Se seu IJogoGeneroRepository tiver apenas o InserirAsync(int, List<int>), 
            // você provavelmente tem um método para deletar (ex: ExcluirPorJogoAsync).
            // Se o build reclamar aqui, me avise qual método de exclusão você tem no JogoGeneroRepository.
            await _repository.JogoGenero.ExcluirPorJogoAsync(jogo.Id); 

            if (idsGeneros.Any())
            {
                await _repository.JogoGenero.InserirAsync(jogo.Id, idsGeneros.ToList());
            }

            transactionScope.Complete();
        }

        return jogo;
    }

    public async Task DesativarAsync(int id)
    {
        var jogo = await _repository.Jogo.BuscarPorIdAsync(id);
        if (jogo == null)
        {
            throw new BusinessRuleException(BusinessErrors.JOGO_NAO_ENCONTRADO);
        }

        await _repository.Jogo.DesativarAsync(id);
    }

    private async Task<IEnumerable<JogoDetalheServiceModel>> MontarDetalhesAsync(IEnumerable<Jogo> jogos)
    {
        var listaJogos = jogos.ToList();
        if (!listaJogos.Any()) return new List<JogoDetalheServiceModel>();

        var plataformas = await _repository.Plataforma.ListarAsync();
        var condicoes = await _repository.CondicaoJogo.ListarAsync(); // Usando Listar em vez de BuscarPorId
        var todosGeneros = await _repository.Genero.ListarAsync(); // Usando Listar em vez de ListarTodos
        
        var todosVinculos = await _repository.JogoGenero.ListarAsync();
        var idsJogosDaPagina = listaJogos.Select(j => j.Id).ToHashSet();
        var vinculosDaPagina = todosVinculos.Where(v => idsJogosDaPagina.Contains(v.IdJogo)).ToList();

        var detalhes = from jogo in listaJogos
                       join plataforma in plataformas on jogo.IdPlataforma equals plataforma.Id
                       join condicao in condicoes on jogo.IdCondicaoJogo equals condicao.Id
                       select new JogoDetalheServiceModel
                       {
                           Jogo = jogo,
                           NomePlataforma = plataforma.Nome,
                           DescricaoCondicao = condicao.Descricao,
                           Generos = (from vinculo in vinculosDaPagina
                                      where vinculo.IdJogo == jogo.Id
                                      join genero in todosGeneros on vinculo.IdGenero equals genero.Id
                                      select genero).ToList()
                       };

        return detalhes;
    }

    private async Task ValidarReferenciasAsync(Jogo jogo, IEnumerable<int> idsGeneros)
    {
        // Para validar a condição sem o BuscarPorIdAsync, buscamos a lista e verificamos com LINQ
        var condicoes = await _repository.CondicaoJogo.ListarAsync();
        if (!condicoes.Any(c => c.Id == jogo.IdCondicaoJogo))
            throw new BusinessRuleException(BusinessErrors.CONDICAO_NAO_ENCONTRADA);

        var plataforma = await _repository.Plataforma.BuscarPorIdAsync(jogo.IdPlataforma);
        if (plataforma == null)
            throw new BusinessRuleException(BusinessErrors.PLATAFORMA_NAO_ENCONTRADA);

        var todosGeneros = await _repository.Genero.ListarAsync();
        var generosValidos = todosGeneros.Select(g => g.Id).ToHashSet();
        
        foreach (var idGenero in idsGeneros)
        {
            if (!generosValidos.Contains(idGenero))
                throw new BusinessRuleException(BusinessErrors.GENERO_NAO_ENCONTRADO);
        }
    }
}