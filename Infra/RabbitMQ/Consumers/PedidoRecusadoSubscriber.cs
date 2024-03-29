﻿using Domain.Pedidos;
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
    public class PedidoRecusadoSubscriber : RabbitMQSubscriber
    {
        public PedidoRecusadoSubscriber(IServiceScopeFactory scopeFactory, IOptions<Secrets> options, IModel model) : base(options.Value.ExchangePedidoRecusado, options.Value.QueuePedidoRecusado, scopeFactory, model) { }

        protected override void InvokeCommand(PedidoDto pedidoDto, IMediatorHandler mediatorHandler)
        {
            var input = new PedidoInput(pedidoDto);
            if (!string.IsNullOrEmpty(input.PedidoDto.ClienteEmail))
            {
                var command = new NotificaPedidoPagamentoReprovadoCommand(input);
                mediatorHandler.EnviarComando<NotificaPedidoPagamentoReprovadoCommand, NotificacaoEnviadaPedidoDto?>(command).Wait();
            }
        }
    }
}
