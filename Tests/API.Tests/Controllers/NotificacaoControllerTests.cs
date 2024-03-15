using API.Controllers;
using Application.Pedidos.DTO;
using Domain.Base.Communication.Mediator;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using System.Security.Claims;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
using Domain.Base.Messages.CommonMessages.Notifications;

namespace API.Tests.Controllers
{
    public class NotificacaoControllerTests
    {
        [Fact]
        public async Task ConsultarNotificacaoPedido_WithValidId_ShouldReturn_NotificationDto()
        {
            // Arrange
            var pedidoId = Guid.NewGuid();
            var notificacaoDto = new NotificacaoEnviadaPedidoDto
            {
                PedidoId = pedidoId,
                Mensagem = "Pedido pronto",
                Email = "test@email.com",
                DataEnvio = DateTime.Now,
                StatusEnvioEmail = true
            };

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddMemoryCache();
            serviceCollection.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var domainNotificationHandler = serviceProvider.GetRequiredService<INotificationHandler<DomainNotification>>();

            var memoryCache = serviceProvider.GetRequiredService<IMemoryCache>();
            var memoryCacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
                SlidingExpiration = TimeSpan.FromMinutes(3)
            };

            memoryCache.Set(pedidoId, notificacaoDto, memoryCacheEntryOptions);


            var mockMediatorHandler = new Mock<IMediatorHandler>();
            var controller = new NotificacaoController(domainNotificationHandler, mockMediatorHandler.Object, memoryCache);
            var defaultHttpContext = new DefaultHttpContext { User = ClaimsPrincipal() };
            controller.ControllerContext = new ControllerContext { HttpContext = defaultHttpContext };


            // Act
            var result = await controller.ConsultarNotificacaoPedido(pedidoId) as OkObjectResult;

            // Assert
            Assert.Equal(StatusCodes.Status200OK, result?.StatusCode);

            var notificacaoResult = Assert.IsType<NotificacaoEnviadaPedidoDto>(result.Value);
            Assert.Equal(pedidoId, notificacaoResult.PedidoId);

        }

        [Fact]
        public async Task ConsultarNotificacaoPedido_WithWrongId_ShouldReturn_NotFound()
        {
            // Arrange
            var pedidoId = Guid.NewGuid();
            var notificacaoDto = new NotificacaoEnviadaPedidoDto
            {
                PedidoId = pedidoId,
                Mensagem = "Pedido pronto",
                Email = "test@email.com",
                DataEnvio = DateTime.Now,
                StatusEnvioEmail = true
            };

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddMemoryCache();
            serviceCollection.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var domainNotificationHandler = serviceProvider.GetRequiredService<INotificationHandler<DomainNotification>>();

            var memoryCache = serviceProvider.GetRequiredService<IMemoryCache>();
            var memoryCacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
                SlidingExpiration = TimeSpan.FromMinutes(3)
            };

            memoryCache.Set(pedidoId, notificacaoDto, memoryCacheEntryOptions);


            var mockMediatorHandler = new Mock<IMediatorHandler>();
            var controller = new NotificacaoController(domainNotificationHandler, mockMediatorHandler.Object, memoryCache);
            var defaultHttpContext = new DefaultHttpContext { User = ClaimsPrincipal() };
            controller.ControllerContext = new ControllerContext { HttpContext = defaultHttpContext };

            var wrongPedidoId = Guid.NewGuid();
            // Act
            var result = await controller.ConsultarNotificacaoPedido(wrongPedidoId) as NotFoundResult;

            // Assert
            Assert.Equal(StatusCodes.Status404NotFound, result?.StatusCode);


        }

        private ClaimsPrincipal ClaimsPrincipal()
        {
            var fakeUserId = Guid.NewGuid().ToString();
            var claims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, fakeUserId) };
            var identity = new ClaimsIdentity(claims);
            var claimsPrincipal = new ClaimsPrincipal(identity);

            return claimsPrincipal;
        }


    }
}
