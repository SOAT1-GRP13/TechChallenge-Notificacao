using Application.Pedidos.Boundaries;
using Application.Pedidos.Commands;
using Application.Pedidos.DTO;
using AutoMapper;
using Domain.Base.Communication.Mediator;
using Domain.Base.Messages.CommonMessages.Notifications;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IMapper _mapper;

        public NotificacaoController(INotificationHandler<DomainNotification> notifications,
            IMediatorHandler mediatorHandler,
            IMemoryCache memoryCache,
            IMapper mapper) : base(notifications, mediatorHandler)
        {
            _mediatorHandler = mediatorHandler;
            _memoryCache = memoryCache;
            _mapper = mapper;
        }

        [HttpGet("consultar-notificacao-pedido/{pedidoId}")]
        //[Authorize]
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

        [HttpPost("enviar-notificacao-pagamento-aprovado-pedido")]
        //[Authorize]
        public async Task<IActionResult> EnviarNotificacaoPagamentoAprovadoPedido(NotificacaoPedidoDto pedidoNotificar)
        {
            if (_memoryCache.TryGetValue(pedidoNotificar.PedidoId, out string notificacao))
            {
                return Ok($"{notificacao}, Enviada a menos de 5 minutos tente novamente mais tarde");
            }
            var pedido = _mapper.Map<NotificacaoPedidoDto, PedidoDto>(pedidoNotificar);
            var input = new PedidoInput(pedido);
            var command = new NotificaPedidoPagamentoAprovadoCommand(input);
            var notificacaoEnviada = await _mediatorHandler.EnviarComando<NotificaPedidoPagamentoAprovadoCommand, NotificacaoEnviadaPedidoDto?>(command);

            if (!OperacaoValida())
                return StatusCode(StatusCodes.Status400BadRequest, ObterMensagensErro());

            return Ok(notificacaoEnviada);
        }

        [HttpPost("enviar-notificacao-pagamento-reprovado-pedido")]
        //[Authorize]
        public async Task<IActionResult> EnviarNotificacaoPagamentoReprovadoPedido(NotificacaoPedidoDto pedidoNotificar)
        {
            if (_memoryCache.TryGetValue(pedidoNotificar.PedidoId, out string notificacao))
            {
                return Ok($"{notificacao}, Enviada a menos de 5 minutos tente novamente mais tarde");
            }

            var pedido = _mapper.Map<NotificacaoPedidoDto, PedidoDto>(pedidoNotificar);
            var input = new PedidoInput(pedido);
            var command = new NotificaPedidoPagamentoAprovadoCommand(input);
            var notificacaoEnviada = await _mediatorHandler.EnviarComando<NotificaPedidoPagamentoAprovadoCommand, NotificacaoEnviadaPedidoDto?>(command);

            if (!OperacaoValida())
                return StatusCode(StatusCodes.Status400BadRequest, ObterMensagensErro());

            return Ok(notificacaoEnviada);
        }
    }
}
