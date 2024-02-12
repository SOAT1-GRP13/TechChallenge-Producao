using Domain.Base.Messages;
using Application.Pedidos.DTO;
using Application.Pedidos.Boundaries;
using Application.Pedidos.Commands.Validation;

namespace Application.Pedidos.Commands
{
    public class PedidoProntoCommand : Command<PedidoDto?>
    {
        public AtualizarStatusPedidoInput Input { get; set; }

        public PedidoProntoCommand(AtualizarStatusPedidoInput input)
        {
            Input = input;
        }

        public override bool EhValido()
        {
            ValidationResult = new AtualizarStatusPedidoValidation().Validate(Input);
            return ValidationResult.IsValid;
        }
    }
}
