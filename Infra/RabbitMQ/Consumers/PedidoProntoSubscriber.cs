using Domain.Pedidos;
using RabbitMQ.Client;
using Application.Pedidos.DTO;
using Application.Pedidos.Commands;
using Application.Pedidos.Boundaries;
using Domain.Base.Communication.Mediator;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Domain.Configuration;

namespace Infra.RabbitMQ.Consumers
{
    public class PedidoProntoSubscriber : RabbitMQSubscriber
    {
        public PedidoProntoSubscriber(IServiceScopeFactory scopeFactory, IOptions<Secrets> options, IModel model) : base(options.Value.ExchangePedidoPronto, options.Value.QueuePedidoPronto, scopeFactory, model) { }

        protected override void InvokeCommand(PedidoDto pedidoDto, IMediatorHandler mediatorHandler)
        {
            var input = new PedidoInput(pedidoDto);
            if (!string.IsNullOrEmpty(input.PedidoDto.ClienteEmail))
            {
                var command = new NotificaPedidoProntoCommand(input);
                mediatorHandler.EnviarComando<NotificaPedidoProntoCommand, NotificacaoEnviadaPedidoDto?>(command).Wait();
            }
        }
    }
}
