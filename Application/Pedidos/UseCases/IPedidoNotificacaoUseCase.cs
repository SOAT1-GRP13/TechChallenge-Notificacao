using Domain.Pedidos;
using Application.Pedidos.DTO;

namespace Application.Pedidos.UseCases
{
    public interface IPedidoNotificacaoUseCase : IDisposable
    {
        Task<NotificacaoEnviadaPedidoDto> NotificaPagamentoAprovadoPedido(PedidoDto pedidoDto);

        Task<NotificacaoEnviadaPedidoDto> NotificaPagamentoReprovadoPedido(PedidoDto pedidoDto);

        Task<NotificacaoEnviadaPedidoDto> NotificaPedidoPronto(PedidoDto pedidoDto);

    }
}
