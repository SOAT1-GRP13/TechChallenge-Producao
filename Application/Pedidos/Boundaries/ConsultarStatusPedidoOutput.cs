using Domain.Pedidos;
using Swashbuckle.AspNetCore.Annotations;

namespace Application.Pedidos.Boundaries
{
    public class ConsultarStatusPedidoOutput
    {
        public ConsultarStatusPedidoOutput()
        {
            Status = PedidoStatus.Iniciado;
        }

        public ConsultarStatusPedidoOutput(PedidoStatus status, Guid pedidoId)
        {
            Status = status;
            PedidoId = pedidoId;
        }

        [SwaggerSchema(
            Title = "Guid do pedido",
            Format = "Guid")]
        public Guid PedidoId { get; set; }

        [SwaggerSchema(
            Title = "Status",
            Format = "enum")]
        public PedidoStatus Status { get; set; }

        [SwaggerSchema(
            Title = "Desc Status",
            Format = "string")]
        public string DescStatus => Status.ToString();
    }
}