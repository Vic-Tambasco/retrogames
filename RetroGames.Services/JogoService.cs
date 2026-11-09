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
        // 1. Se o filtro tiver gênero, descobre quais jogos têm esse gênero
        if (filtro.IdGenero.HasValue)
        {
            var vinculos = await _repository.JogoGenero.ListarPorGeneroAsync(filtro.IdGenero.Value);
            filtro.IdsJogosFiltrados = vinculos.Select(v => v.IdJogo).ToList();
            
            if (!filtro.IdsJogosFiltrados.Any())
            {
                return (new List<JogoDetalheServiceModel>(), 0);
            }
        }

        // 2. Busca a página de jogos no Repository (com paginação no SQL)
        var resultado = await _repository.Jogo.ListarAsync(filtro);
        
        // 3. Monta o detalhe com LINQ (número fixo de consultas)
        var detalhes = await MontarDetalhesAsync(resultado.Items);
        
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
        jogo.Ativo = true; // RN03: jogo novo entra ativo

        // RN05: Valida referências antes de gravar
        await ValidarReferenciasAsync(jogo, idsGeneros);

        var transactionOptions = new TransactionOptions
        {
            IsolationLevel = IsolationLevel.ReadCommitted,
            Timeout = TransactionManager.DefaultTimeout,
        };

        using (var transactionScope = new TransactionScope(TransactionScopeOption.Required,
                   transactionOptions, TransactionScopeAsyncFlowOption.Enabled))
        {
            // Grava o jogo
            jogo.Id = await _repository.Jogo.InserirAsync(jogo);

            // Grava os vínculos de gênero
            foreach (var idGenero in idsGeneros)
            {
                var vinculo = new JogoGenero { IdJogo = jogo.Id, IdGenero = idGenero };
                await _repository.JogoGenero.InserirAsync(vinculo);
            }

            transactionScope.Complete();
        }

        return jogo;
    }

    public async Task<Jogo> AlterarAsync(Jogo jogo, IEnumerable<int> idsGeneros)
    {
        // Verifica se o jogo existe
        var jogoExistente = await _repository.Jogo.BuscarPorIdAsync(jogo.Id);
        if (jogoExistente == null)
        {
            throw new BusinessRuleException(BusinessErrors.JOGO_NAO_ENCONTRADO);
        }

        jogo.Nome = jogo.Nome.Trim();
        
        // RN05: Valida referências
        await ValidarReferenciasAsync(jogo, idsGeneros);

        var transactionOptions = new TransactionOptions
        {
            IsolationLevel = IsolationLevel.ReadCommitted,
            Timeout = TransactionManager.DefaultTimeout,
        };

        using (var transactionScope = new TransactionScope(TransactionScopeOption.Required,
                   transactionOptions, TransactionScopeAsyncFlowOption.Enabled))
        {
            // Altera o jogo
            await _repository.Jogo.AlterarAsync(jogo);

            // Remove vínculos antigos
            await _repository.JogoGenero.ExcluirPorJogoAsync(jogo.Id);

            // Grava novos vínculos
            foreach (var idGenero in idsGeneros)
            {
                var vinculo = new JogoGenero { IdJogo = jogo.Id, IdGenero = idGenero };
                await _repository.JogoGenero.InserirAsync(vinculo);
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

        // RN03: Jogo não é apagado, apenas desativado (st_ativo = 0)
        jogo.Ativo = false;
        await _repository.Jogo.AlterarAsync(jogo);
    }

    // --- Métodos Privados de Apoio ---

    private async Task<IEnumerable<JogoDetalheServiceModel>> MontarDetalhesAsync(IEnumerable<Jogo> jogos)
    {
        var listaJogos = jogos.ToList();
        if (!listaJogos.Any()) return new List<JogoDetalheServiceModel>();

        // Consultas fixas (independente do tamanho da lista)
        var plataformas = await _repository.Plataforma.ListarAsync();
        var condicoes = await _repository.CondicaoJogo.ListarAsync();
        var todosGeneros = await _repository.Genero.ListarTodosAsync();
        
        // Para os gêneros, precisamos buscar os vínculos apenas dos jogos da lista
        var idsJogos = listaJogos.Select(j => j.Id).ToList();
        var vinculosGeneros = await _repository.JogoGenero.ListarPorJogosAsync(idsJogos);

        // Montagem com LINQ em memória
        var detalhes = from jogo in listaJogos
                       join plataforma in plataformas on jogo.IdPlataforma equals plataforma.Id
                       join condicao in condicoes on jogo.IdCondicaoJogo equals condicao.Id
                       select new JogoDetalheServiceModel
                       {
                           Jogo = jogo,
                           NomePlataforma = plataforma.Nome,
                           DescricaoCondicao = condicao.Descricao,
                           Generos = (from vinculo in vinculosGeneros
                                      where vinculo.IdJogo == jogo.Id
                                      join genero in todosGeneros on vinculo.IdGenero equals genero.Id
                                      select genero).ToList()
                       };

        return detalhes;
    }

    private async Task ValidarReferenciasAsync(Jogo jogo, IEnumerable<int> idsGeneros)
    {
        // RN05: Valida se a plataforma existe
        var plataforma = await _repository.Plataforma.BuscarPorIdAsync(jogo.IdPlataforma);
        if (plataforma == null)
        {
            throw new BusinessRuleException(BusinessErrors.PLATAFORMA_NAO_ENCONTRADA);
        }

        // RN05: Valida se a condição existe
        var condicao = await _repository.CondicaoJogo.BuscarPorIdAsync(jogo.IdCondicaoJogo);
        if (condicao == null)
        {
            throw new BusinessRuleException(BusinessErrors.CONDICAO_NAO_ENCONTRADA);
        }

        // RN05: Valida se todos os gêneros existem
        var todosGeneros = await _repository.Genero.ListarTodosAsync();
        var generosValidos = todosGeneros.Select(g => g.Id).ToHashSet();
        
        foreach (var idGenero in idsGeneros)
        {
            if (!generosValidos.Contains(idGenero))
            {
                throw new BusinessRuleException(BusinessErrors.GENERO_NAO_ENCONTRADO);
            }
        }
    }
}