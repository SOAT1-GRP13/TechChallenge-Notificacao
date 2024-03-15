using MediatR;
using Domain.Pedidos;
using Application.Pedidos.DTO;
using Domain.Base.DomainObjects;
using Application.Pedidos.Commands;
using Application.Pedidos.UseCases;
using Domain.Base.Communication.Mediator;
using Domain.Base.Messages.CommonMessages.Notifications;
using Microsoft.Extensions.Caching.Memory;

namespace Application.Pedidos.Handlers
{
    public class NotificaPedidoPagamentoReprovadoCommandHandler : IRequestHandler<NotificaPedidoPagamentoReprovadoCommand, NotificacaoEnviadaPedidoDto?>
    {
        private readonly IPedidoNotificacaoUseCase _pedidoUseCase;
        private readonly IMediatorHandler _mediatorHandler;
        private IMemoryCache _memoryCache;

        public NotificaPedidoPagamentoReprovadoCommandHandler(
            IPedidoNotificacaoUseCase statusPedidoUseCase,
            IMediatorHandler mediatorHandler,
            IMemoryCache memoryCache
        )
        {
            _pedidoUseCase = statusPedidoUseCase;
            _mediatorHandler = mediatorHandler;
            _memoryCache = memoryCache;
        }

        public async Task<NotificacaoEnviadaPedidoDto?> Handle(NotificaPedidoPagamentoReprovadoCommand request, CancellationToken cancellationToken)
        {
            if (!request.EhValido())
            {
                foreach (var error in request.ValidationResult.Errors)
                    await _mediatorHandler.PublicarNotificacao(new DomainNotification(request.MessageType, error.ErrorMessage));
                return null;
            }

            try
            {
                var input = request.Input;
                var notificacaoEnviada = await _pedidoUseCase.NotificaPagamentoReprovadoPedido(input.PedidoDto);

                if (notificacaoEnviada.StatusEnvioEmail == true)
                {
                    var memoryCacheEntryOptions = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
                        SlidingExpiration = TimeSpan.FromMinutes(3)
                    };

                    if (!_memoryCache.TryGetValue(input.PedidoDto.PedidoId, out NotificacaoEnviadaPedidoDto notificacao))
                    {
                        _memoryCache.Set(notificacaoEnviada.PedidoId, notificacaoEnviada, memoryCacheEntryOptions);
                    }
                }

                return notificacaoEnviada;
            }
            catch (DomainException ex)
            {
                await _mediatorHandler.PublicarNotificacao(new DomainNotification(request.MessageType, ex.Message));
                return null;
            }
        }
    }
}
