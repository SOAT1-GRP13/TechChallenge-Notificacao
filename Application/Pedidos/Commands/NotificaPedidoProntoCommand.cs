using Domain.Base.Messages;
using Application.Pedidos.DTO;
using Application.Pedidos.Boundaries;
using Application.Pedidos.Commands.Validation;

namespace Application.Pedidos.Commands
{
    public class NotificaPedidoProntoCommand : Command<NotificacaoEnviadaPedidoDto?>
    {
        public PedidoInput Input { get; set; }

        public NotificaPedidoProntoCommand(PedidoInput input)
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
