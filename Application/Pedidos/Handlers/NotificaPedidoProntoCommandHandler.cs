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
    public class NotificaPedidoProntoCommandHandler : IRequestHandler<NotificaPedidoProntoCommand, NotificacaoEnviadaPedidoDto?>
    {
        private readonly IPedidoNotificacaoUseCase _pedidoNotificacaoUseCase;
        private readonly IMediatorHandler _mediatorHandler;
        private IMemoryCache _memoryCache;

        public NotificaPedidoProntoCommandHandler(
            IPedidoNotificacaoUseCase pedidoNotificacaoUseCase,
            IMediatorHandler mediatorHandler,
            IMemoryCache memoryCache
        )
        {
            _pedidoNotificacaoUseCase = pedidoNotificacaoUseCase;
            _mediatorHandler = mediatorHandler;
            _memoryCache = memoryCache;
        }

        public async Task<NotificacaoEnviadaPedidoDto?> Handle(NotificaPedidoProntoCommand request, CancellationToken cancellationToken)
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
                var notificacaoEnviada = await _pedidoNotificacaoUseCase.NotificaPedidoPronto(input.PedidoDto);

                if (notificacaoEnviada.StatusEnvioEmail == true)
                {
                    var memoryCacheEntryOptions = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
                        SlidingExpiration = TimeSpan.FromMinutes(3)
                    };

                    _memoryCache.Set(notificacaoEnviada.PedidoId, notificacaoEnviada, memoryCacheEntryOptions);
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
