using System.Transactions;
using RetroGames.Model.Model;
using RetroGames.Repository.Interfaces;

namespace RetroGames.Services;

public class PedidoService : IPedidoService
{
    private readonly IRepositorySet _repository;

    public PedidoService(IRepositorySet repository)
    {
        _repository = repository;
    }

    public async Task<Pedido> FinalizarCompraAsync(int idUsuario, PedidoRequest request)
    {
        if (request.Itens == null || !request.Itens.Any())
            throw new BusinessRuleException(BusinessErrors.PEDIDO_SEM_ITENS);

        var pedido = new Pedido
        {
            DataPedido = DateTime.UtcNow,
            IdUsuario = idUsuario,
            IdSituacaoPedido = 1, // Pendente
            Total = 0
        };

        var itensPedido = new List<ItemPedido>();

        // 1. Validar cada item e calcular o total
        foreach (var itemReq in request.Itens)
        {
            if (itemReq.Quantidade < 1 || itemReq.Quantidade > 10)
                throw new BusinessRuleException(BusinessErrors.QUANTIDADE_PEDIDO_INVALIDA);

            var jogo = await _repository.Jogo.BuscarPorIdAsync(itemReq.IdJogo);
            
            if (jogo == null || !jogo.Ativo)
                throw new BusinessRuleException(BusinessErrors.JOGO_INDISPONIVEL);

            if (jogo.Estoque < itemReq.Quantidade)
                throw new BusinessRuleException(BusinessErrors.ESTOQUE_INSUFICIENTE);

            var item = new ItemPedido
            {
                IdJogo = jogo.Id,
                Quantidade = itemReq.Quantidade,
                ValorUnitario = jogo.Preco // Preço vem do banco!
            };
            
            itensPedido.Add(item);
            pedido.Total += (item.ValorUnitario * item.Quantidade);
        }

        // 2. Transação para gravar tudo
        var transactionOptions = new TransactionOptions
        {
            IsolationLevel = IsolationLevel.ReadCommitted,
            Timeout = TransactionManager.DefaultTimeout,
        };

        using (var transactionScope = new TransactionScope(TransactionScopeOption.Required,
                   transactionOptions, TransactionScopeAsyncFlowOption.Enabled))
        {
            // Grava o pedido
            pedido.Id = await _repository.Pedido.InserirAsync(pedido);

            // Grava os itens e baixa o estoque
            foreach (var item in itensPedido)
            {
                item.IdPedido = pedido.Id;
                await _repository.ItemPedido.InserirAsync(item);
                
                var jogo = await _repository.Jogo.BuscarPorIdAsync(item.IdJogo);
                if (jogo == null)
                    throw new BusinessRuleException(BusinessErrors.JOGO_INDISPONIVEL);

                jogo.Estoque -= item.Quantidade;
                await _repository.Jogo.AlterarAsync(jogo);
            }

            transactionScope.Complete();
        }

        return pedido;
    }

    public async Task<IEnumerable<PedidoDetalheServiceModel>> ListarMeusPedidosAsync(int idUsuario)
    {
        var pedidos = await _repository.Pedido.ListarPorUsuarioAsync(idUsuario);
        var situacoes = await _repository.SituacaoPedido.ListarAsync();
        var resultado = new List<PedidoDetalheServiceModel>();

        foreach (var pedido in pedidos)
        {
            var itens = await _repository.ItemPedido.ListarPorPedidoAsync(pedido.Id);
            var nomesJogos = new Dictionary<int, string>();

            foreach (var idJogo in itens.Select(item => item.IdJogo).Distinct())
            {
                var jogo = await _repository.Jogo.BuscarPorIdAsync(idJogo);
                if (jogo != null)
                    nomesJogos[idJogo] = jogo.Nome;
            }

            var situacao = situacoes.FirstOrDefault(s => s.Id == pedido.IdSituacaoPedido);

            var detalhe = new PedidoDetalheServiceModel
            {
                Pedido = pedido,
                DescricaoSituacao = situacao?.Descricao ?? "Desconhecida",
                Itens = itens.Select(i => new ItemPedidoDetalhe
                {
                    NomeJogo = nomesJogos.GetValueOrDefault(i.IdJogo, "Jogo Removido"),
                    Quantidade = i.Quantidade,
                    ValorUnitario = i.ValorUnitario
                }).ToList()
            };
            
            resultado.Add(detalhe);
        }

        return resultado;
    }
}