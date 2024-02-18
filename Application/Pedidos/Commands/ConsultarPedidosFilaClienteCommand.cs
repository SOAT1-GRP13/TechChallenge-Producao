using Domain.Base.Messages;
using Application.Pedidos.Boundaries;

namespace Application.Pedidos.Commands
{
    public class ConsultarPedidosFilaClienteCommand : Command<IEnumerable<ConsultarPedidosFilaClienteOutput>>
    {
    }
}
