using Application.Pedidos.Boundaries;
using Application.Pedidos.Commands;
using Application.Pedidos.DTO;
using Application.Pedidos.Handlers;
using Application.Pedidos.UseCases;
using Domain.Base.Communication.Mediator;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Tests.Pedidos.Handlers
{
    public class NotificaPedidoPagamentoReprovadoCommandHandlerTests
    {

        [Fact]
        public async Task Handle_ValidCommand_Returns_NotificacaoEnviadaPedidoDto()
        {
            // Arrange
            var pedidoDto = new PedidoDto
            {
                PedidoId = Guid.NewGuid(),
                ClienteEmail = "client@example.com"
            };

            var pedidoInput = new PedidoInput(pedidoDto);

            var command = new NotificaPedidoPagamentoReprovadoCommand(pedidoInput);
            var mockPedidoNotificacaoUseCase = new Mock<IPedidoNotificacaoUseCase>();
            mockPedidoNotificacaoUseCase.Setup(useCase => useCase.NotificaPagamentoReprovadoPedido(pedidoDto))
                                        .ReturnsAsync(new NotificacaoEnviadaPedidoDto
                                        {
                                            PedidoId = pedidoDto.PedidoId,
                                            Email = pedidoDto.ClienteEmail,
                                            Mensagem = "O Pagamento do seu pedido foi reprovado",
                                            DataEnvio = DateTime.Now,
                                            StatusEnvioEmail = true
                                        });

            var mockMediatorHandler = new Mock<IMediatorHandler>();
            var mockMemoryCache = new Mock<IMemoryCache>();

            mockMemoryCache.Setup(m => m.TryGetValue(It.IsAny<object>(), out It.Ref<object>.IsAny))
               .Returns(false);

            mockMemoryCache.Setup(m => m.CreateEntry(It.IsAny<object>()))
                           .Returns<object>(key =>
                           {
                               var mockCacheEntry = new Mock<ICacheEntry>();
                               return mockCacheEntry.Object;
                           });

            var handler = new NotificaPedidoPagamentoReprovadoCommandHandler(
                mockPedidoNotificacaoUseCase.Object,
                mockMediatorHandler.Object,
                mockMemoryCache.Object
            );

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(pedidoDto.PedidoId, result.PedidoId);
            Assert.Equal(pedidoDto.ClienteEmail, result.Email);
            Assert.Equal("O Pagamento do seu pedido foi reprovado", result.Mensagem);
            Assert.True(result.DataEnvio <= DateTime.Now);
            Assert.True(result.StatusEnvioEmail);
        }
    }
}
