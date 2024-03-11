using Domain.Pedidos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Pedidos.DTO
{
    public class PedidoDto
    {
        public Guid PedidoId { get; set; }
        public Guid ClienteId { get; set; }
        public DateTime DataCadastro { get; set; }
        public PedidoStatus PedidoStatus { get; set; }

    }
}
