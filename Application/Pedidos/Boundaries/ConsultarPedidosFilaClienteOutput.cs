using Domain.Pedidos;
using Swashbuckle.AspNetCore.Annotations;

namespace Application.Pedidos.Boundaries
{
    public class ConsultarPedidosFilaClienteOutput
    {
        public ConsultarPedidosFilaClienteOutput()
        {
            PedidoId = Guid.Empty;
            ClientId = Guid.Empty;
        }

        public ConsultarPedidosFilaClienteOutput(Guid pedidoId, Guid clientId, PedidoStatus status)
        {            
            PedidoId = pedidoId;
            ClientId = clientId;
            Status = status;
        }

        [SwaggerSchema(
            Title = "Guid do pedido",
            Format = "Guid")]
        public Guid PedidoId { get; set; }

        [SwaggerSchema(
            Title = "Guid do cliente",
            Format = "Guid")]
        public Guid ClientId { get; set; }

        [SwaggerSchema(
            Title = "Status",
            Format = "enum")]
        public PedidoStatus Status { get; set; }
    }
}
