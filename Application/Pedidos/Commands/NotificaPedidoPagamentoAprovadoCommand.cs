using Domain.Base.Messages;
using Application.Pedidos.DTO;
using Application.Pedidos.Boundaries;
using Application.Pedidos.Commands.Validation;

namespace Application.Pedidos.Commands
{
    public class NotificaPedidoPagamentoAprovadoCommand : Command<NotificacaoEnviadaPedidoDto?>
    {
        public PedidoInput Input { get; set; }

        public NotificaPedidoPagamentoAprovadoCommand(PedidoInput input)
        {
            Input = input;
        }

        public override bool EhValido()
        {
            ValidationResult = new PedidoValidation().Validate(Input);
            return ValidationResult.IsValid;
        }
    }
}
