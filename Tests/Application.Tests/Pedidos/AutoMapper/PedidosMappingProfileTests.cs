using Application.Pedidos.AutoMapper;
using Application.Pedidos.DTO;
using AutoMapper;
using Domain.Pedidos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Tests.Pedidos.AutoMapper
{
    public class PedidosMappingProfileTests
    {
        private readonly IMapper _mapper;

        public PedidosMappingProfileTests()
        {
            // Arrange
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<PedidosMappingProfile>();
            });
            _mapper = configuration.CreateMapper();
        }

        [Fact]
        public void DeveMapearPedidoDtoParaNotificacaoPedidoDtoCorretamente()
        {
            // Arrange
            var pedidoDto = new PedidoDto
            {
                PedidoId = Guid.NewGuid(),
                ClienteEmail = "client@example.com"
            };

            // Act
            var notificacaoPedidoDto = _mapper.Map<NotificacaoPedidoDto>(pedidoDto);

            // Assert
            Assert.Equal(pedidoDto.PedidoId, notificacaoPedidoDto.PedidoId);
        }
    }
}
