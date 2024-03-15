using Application.Pedidos.DTO;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Application.Pedidos.Boundaries
{
    public class PedidoInput
    {
        public PedidoInput(PedidoDto pedido)
        {
            PedidoDto = pedido;
        }

        [Required]
        [SwaggerSchema(
            Title = "Guid do pedido",
            Format = "Guid")]
        public PedidoDto PedidoDto { get; set; }

    }
}