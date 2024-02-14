using FluentValidation;
using Application.Pedidos.Boundaries;

namespace Application.Pedidos.Commands.Validation
{
    public class AdicionarPedidoValidation : AbstractValidator<AdicionarPedidoInput>
    {
        public AdicionarPedidoValidation()
        {
            RuleFor(a => a.pedidoDto.PedidoId)
                .NotEmpty()
                .WithMessage("Id do pedido é obrigatório");
        }
    }
}
