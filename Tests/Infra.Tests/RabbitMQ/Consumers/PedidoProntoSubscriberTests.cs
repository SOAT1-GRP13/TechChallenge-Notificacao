﻿using Domain.Base.Communication.Mediator;
using Domain.Configuration;
using Infra.RabbitMQ.Consumers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Tests.RabbitMQ.Consumers
{
    public class PedidoProntoSubscriberTests
    {
        private readonly Mock<IOptions<Secrets>> _mockOptions;
        private readonly Secrets _secrets;
        public PedidoProntoSubscriberTests()
        {
            _mockOptions = new Mock<IOptions<Secrets>>();
            _secrets = new Secrets()
            {
                ExchangePedidoPronto = "exc_pedido_pronto",
                QueuePedidoPronto = "pedido_pronto_pedido"

            };
            _mockOptions.Setup(opt => opt.Value).Returns(_secrets);

        }

        [Fact]
        public void AoExecuteAsync_SeNaoConseguirDesSerializarDto_DeveLancarExcessao()
        {
            // Arrange
            var mockModel = new Mock<IModel>();
            var mockScopeFactory = new Mock<IServiceScopeFactory>();
            var mockScope = new Mock<IServiceScope>();
            var mockServiceProvider = new Mock<IServiceProvider>();
            var mockMediatorHandler = new Mock<IMediatorHandler>();

            mockScopeFactory.Setup(x => x.CreateScope()).Returns(mockScope.Object);
            mockScope.Setup(x => x.ServiceProvider).Returns(mockServiceProvider.Object);
            mockServiceProvider.Setup(x => x.GetService(typeof(IMediatorHandler))).Returns(mockMediatorHandler.Object);

            var serverFake = new PedidoProntoSubscriberFake(mockScopeFactory.Object, _mockOptions.Object, mockModel.Object);
            var consumer = new EventingBasicConsumer(mockModel.Object);

            // Simula um evento Received com um corpo de mensagem inválido
            var eventArgs = new BasicDeliverEventArgs
            {
                Body = Encoding.UTF8.GetBytes(string.Empty)
            };

            Exception? capturedException = null;
            consumer.Received += (model, ea) =>
            {
                try
                {
                    serverFake.InvokeReceivedEvent(model, ea);
                }
                catch (Exception ex)
                {
                    capturedException = ex;
                }
            };

            // Act
            consumer.HandleBasicDeliver(eventArgs.ConsumerTag, eventArgs.DeliveryTag, eventArgs.Redelivered, eventArgs.Exchange, eventArgs.RoutingKey, eventArgs.BasicProperties, eventArgs.Body);

            // Assert
            Assert.NotNull(capturedException);
            Assert.Equal("Erro deserializar PedidoDto", capturedException?.Message);
        }

        [Fact]
        public void AoExecuteAsync_SeConseguirDesSerializarDto_DeveExecutarComSucesso()
        {
            // Arrange
            var mockModel = new Mock<IModel>();
            var mockScopeFactory = new Mock<IServiceScopeFactory>();
            var mockScope = new Mock<IServiceScope>();
            var mockServiceProvider = new Mock<IServiceProvider>();
            var mockMediatorHandler = new Mock<IMediatorHandler>();

            mockScopeFactory.Setup(x => x.CreateScope()).Returns(mockScope.Object);
            mockScope.Setup(x => x.ServiceProvider).Returns(mockServiceProvider.Object);
            mockServiceProvider.Setup(x => x.GetService(typeof(IMediatorHandler))).Returns(mockMediatorHandler.Object);

            var serverFake = new PedidoProntoSubscriberFake(mockScopeFactory.Object, _mockOptions.Object, mockModel.Object);
            var consumer = new EventingBasicConsumer(mockModel.Object);

            var eventArgs = new BasicDeliverEventArgs
            {
                Body = Encoding.UTF8.GetBytes("{}")
            };

            Exception? capturedException = null;
            consumer.Received += (model, ea) =>
            {
                try
                {
                    serverFake.InvokeReceivedEvent(model, ea);
                }
                catch (Exception ex)
                {
                    Assert.True(false, ex.Message);
                }
            };

            // Act
            consumer.HandleBasicDeliver(eventArgs.ConsumerTag, eventArgs.DeliveryTag, eventArgs.Redelivered, eventArgs.Exchange, eventArgs.RoutingKey, eventArgs.BasicProperties, eventArgs.Body);

            // Assert
            Assert.Null(capturedException);
        }

        [Fact]
        public void Dispose_ShouldCloseChannel()
        {
            // Arrange
            var mockModel = new Mock<IModel>();
            var mockScopeFactory = new Mock<IServiceScopeFactory>();

            mockModel.Setup(m => m.IsOpen).Returns(true);

            var subscriber = new PedidoProntoSubscriber(mockScopeFactory.Object, _mockOptions.Object, mockModel.Object);

            // Act
            try
            {
                subscriber.Dispose();
            }
            catch
            {
                Assert.True(false, "Erro ao executar Dispose");
            }

            Assert.True(true);
        }
        private class PedidoProntoSubscriberFake : PedidoProntoSubscriber
        {
            public PedidoProntoSubscriberFake(IServiceScopeFactory scopeFactory, IOptions<Secrets> options, IModel model) : base(scopeFactory, options, model)
            {
            }

            public new void InvokeReceivedEvent(object? model, BasicDeliverEventArgs ea)
            {
                base.InvokeReceivedEvent(model, ea);
            }
        }

    }
}
