using Application.Pedidos.DTO;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Application.Pedidos.Boundaries
{
    public class AdicionarPedidoInput
    {
        public AdicionarPedidoInput(PedidoDto pedido)
        {
            pedidoDto = pedido;
        }

        [Required]
        [SwaggerSchema(
            Title = "Guid do pedido",
            Format = "Guid")]
        public PedidoDto pedidoDto { get; set; }
    }
}