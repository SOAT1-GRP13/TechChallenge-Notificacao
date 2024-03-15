using Application.Common.Interfaces.Services;
using Application.Models.Email;
using Application.Pedidos.DTO;
using Application.Pedidos.UseCases;
using Moq;
using System;

namespace Application.Tests.Pedidos.UseCases
{
    public class PedidoNotificacaoUseCaseTests
    {
        [Fact]
        public async Task NotificaPagamentoAprovadoPedido_Should_Send_Email()
        {
            // Arrange
            var pedidoDto = new PedidoDto
            {
                PedidoId = Guid.NewGuid(),
                ClienteEmail = "client@example.com"
            };

            var mockEmailSender = new Mock<IEmailSender>();
            mockEmailSender.Setup(sender => sender.SendEmailAsync(It.IsAny<EmailMessage>()))
                           .ReturnsAsync(true);

            var useCase = new PedidoNotificacaoUseCase(mockEmailSender.Object);

            // Act
            var result = await useCase.NotificaPagamentoAprovadoPedido(pedidoDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(pedidoDto.PedidoId, result.PedidoId);
            Assert.Equal(pedidoDto.ClienteEmail, result.Email);
            Assert.Equal("O Pagamento do seu pedido foi aprovado", result.Mensagem);
            Assert.True(result.DataEnvio <= DateTime.Now);
            Assert.True(result.StatusEnvioEmail);
        }

        [Fact]
        public async Task NotificaPagamentoReprovadoPedido_Should_Send_Email()
        {
            // Arrange
            var pedidoDto = new PedidoDto
            {
                PedidoId = Guid.NewGuid(),
                ClienteEmail = "client@example.com"
            };

            var mockEmailSender = new Mock<IEmailSender>();
            mockEmailSender.Setup(sender => sender.SendEmailAsync(It.IsAny<EmailMessage>()))
                           .ReturnsAsync(true);

            var useCase = new PedidoNotificacaoUseCase(mockEmailSender.Object);

            // Act
            var result = await useCase.NotificaPagamentoReprovadoPedido(pedidoDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(pedidoDto.PedidoId, result.PedidoId);
            Assert.Equal(pedidoDto.ClienteEmail, result.Email);
            Assert.Equal("O Pagamento do seu pedido foi reprovado", result.Mensagem);
            Assert.True(result.DataEnvio <= DateTime.Now);
            Assert.True(result.StatusEnvioEmail);
        }

        [Fact]
        public async Task NotificaPedidoPronto_Should_Send_Email()
        {
            // Arrange
            var pedidoDto = new PedidoDto
            {
                PedidoId = Guid.NewGuid(),
                ClienteEmail = "client@example.com"
            };

            var mockEmailSender = new Mock<IEmailSender>();
            mockEmailSender.Setup(sender => sender.SendEmailAsync(It.IsAny<EmailMessage>()))
                           .ReturnsAsync(true);

            var useCase = new PedidoNotificacaoUseCase(mockEmailSender.Object);

            // Act
            var result = await useCase.NotificaPedidoPronto(pedidoDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(pedidoDto.PedidoId, result.PedidoId);
            Assert.Equal(pedidoDto.ClienteEmail, result.Email);
            Assert.Equal("O seu pedido está pronto", result.Mensagem);
            Assert.True(result.DataEnvio <= DateTime.Now);
            Assert.True(result.StatusEnvioEmail);
        }
    }
}