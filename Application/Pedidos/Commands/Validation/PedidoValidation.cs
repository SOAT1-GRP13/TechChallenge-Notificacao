using FluentValidation;
using Application.Pedidos.Boundaries;

namespace Application.Pedidos.Commands.Validation
{
    public class PedidoValidation : AbstractValidator<PedidoInput>
    {
        public PedidoValidation()
        {
            RuleFor(a => a.PedidoDto.PedidoId)
                .NotEmpty()
                .WithMessage("Id do pedido é obrigatório");
        }
    }
}
