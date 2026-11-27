using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RetroGames.Services;
using RetroGames.WebApi.ApiModels;

namespace RetroGames.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PedidoController : ControllerBase
{
    private readonly IPedidoService _pedidoService;
    private readonly IUsuarioAtualService _usuarioAtual;

    public PedidoController(IPedidoService pedidoService, IUsuarioAtualService usuarioAtual)
    {
        _pedidoService = pedidoService;
        _usuarioAtual = usuarioAtual;
    }

    [HttpPost]
    public async Task<ActionResult<RetroGamesResponseApiModel<object>>> Finalizar(FinalizarPedidoRequestApiModel request)
    {
        if (!_usuarioAtual.EstaAutenticado())
            return Unauthorized(); // O middleware já barra, mas é bom garantir

        var idUsuario = _usuarioAtual.ObterId();
        
        // Mapeia do ApiModel para o ServiceModel
        var pedidoRequest = new PedidoRequest
        {
            Itens = request.Itens.Select(i => new ItemPedidoRequest { IdJogo = i.IdJogo, Quantidade = i.Quantidade }).ToList()
        };

        var pedido = await _pedidoService.FinalizarCompraAsync(idUsuario, pedidoRequest);

        return Ok(new RetroGamesResponseApiModel<object>(new { idPedido = pedido.Id, total = pedido.Total }));
    }
}