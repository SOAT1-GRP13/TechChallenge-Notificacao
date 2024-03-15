using Application.Pedidos.DTO;
using AutoMapper;
using Domain.Base.Communication.Mediator;
using Domain.Base.Messages.CommonMessages.Notifications;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Controllers
{
    [ApiController]
    [Route("Notificacao")]
    [SwaggerTag("Endpoints relacionados a notificacao")]
    public class NotificacaoController : ControllerBase
    {
        private readonly IMediatorHandler _mediatorHandler;
        private readonly IMemoryCache _memoryCache;

        public NotificacaoController(INotificationHandler<DomainNotification> notifications,
            IMediatorHandler mediatorHandler,
            IMemoryCache memoryCache) : base(notifications, mediatorHandler)
        {
            _mediatorHandler = mediatorHandler;
            _memoryCache = memoryCache;
        }

        [HttpGet("consultar-notificacao-pedido/{pedidoId}")]
        [SwaggerOperation(
            Summary = "Consultar notificacao do pedido",
            Description = "Consulta notificacao do pedido a partir do Guid")]
        [SwaggerResponse(200, "Retorna se notificacao foi enviada")]
        [SwaggerResponse(404, "Caso não encontre o pedido com o Id informado")]
        [SwaggerResponse(500, "Caso algo inesperado aconteça")]
        public async Task<IActionResult> ConsultarNotificacaoPedido([FromRoute] Guid pedidoId)
        {
            if (!_memoryCache.TryGetValue(pedidoId, out NotificacaoEnviadaPedidoDto notificacao))
            {
                await _mediatorHandler.PublicarNotificacao(new DomainNotification("Notificacao", "Notificacao não encontrada"));
                return NotFound();
            }

            if (!OperacaoValida())
                return StatusCode(StatusCodes.Status400BadRequest, ObterMensagensErro());

            return Ok(notificacao);
        }
    }
}
