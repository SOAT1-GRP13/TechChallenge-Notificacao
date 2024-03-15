using Domain.Pedidos;

namespace Application.Pedidos.DTO
{
    public class PedidoDto
    {
        public Guid PedidoId { get; set; }
        public string ClienteEmail { get; set; } = string.Empty;

    }
}
