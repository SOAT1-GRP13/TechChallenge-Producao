using Domain.Base.Messages;
using Application.Pedidos.DTO;

namespace Application.Pedidos.Commands
{
    public class ConsultarPedidosFilaProducaoCommand : Command<IEnumerable<PedidoDto>>
    {
    }
}
