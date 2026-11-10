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

            // Remove os vínculos antigos usando o método exato do seu repositório
            await _repository.JogoGenero.RemoverPorJogoAsync(jogo.Id);

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
        var condicoes = await _repository.CondicaoJogo.ListarAsync(); 
        var todosGeneros = await _repository.Genero.ListarAsync(); 
        
        // Busca os vínculos de forma otimizada usando o método exato do seu repositório
        var idsJogos = listaJogos.Select(j => j.Id).ToList();
        var vinculosDaPagina = await _repository.JogoGenero.ListarPorJogosAsync(idsJogos);

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