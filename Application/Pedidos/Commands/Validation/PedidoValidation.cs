using FluentValidation;
using Application.Pedidos.Boundaries;

namespace Application.Pedidos.Commands.Validation
{
    public class PedidoValidation : AbstractValidator<PedidoInput>
    {
        public PedidoValidation()
        {
            RuleFor(a => a.pedidoDto.PedidoId)
                .NotEmpty()
                .WithMessage("Id do pedido é obrigatório");

            RuleFor(a => a.pedidoDto.ClienteId)
                .NotEmpty()
                .WithMessage("Id do cliente é obrigatório para envio notificação");
        }
    }
}
