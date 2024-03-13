using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Pedidos.DTO
{
    public class NotificacaoPedidoDto
    {
        public Guid PedidoId { get; set; }
        public Guid ClienteId { get; set; }

    }
}
