using Domain.Base.Messages;
using Application.Pedidos.DTO;
using Application.Pedidos.Boundaries;
using Application.Pedidos.Commands.Validation;

namespace Application.Pedidos.Commands
{
    public class AdicionarPedidoCommand : Command<PedidoDto?>
    {
        public AdicionarPedidoInput Input { get; set; }

        public AdicionarPedidoCommand(AdicionarPedidoInput input)
        {
            Input = input;
        }

        public override bool EhValido()
        {
            ValidationResult = new AdicionarPedidoValidation().Validate(Input);
            return ValidationResult.IsValid;
        }
    }
}
