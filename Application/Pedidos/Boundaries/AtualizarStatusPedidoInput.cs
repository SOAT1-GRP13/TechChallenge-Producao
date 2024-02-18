using Domain.Pedidos;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Application.Pedidos.Boundaries
{
    public class AtualizarStatusPedidoInput
    {
        public AtualizarStatusPedidoInput(Guid idPedido, int status)
        {
            IdPedido = idPedido;
            Status = status;
        }

        [Required]
        [SwaggerSchema(
            Title = "Guid do pedido",
            Format = "Guid")]
        public Guid IdPedido { get; set; }

        [Required]
        [SwaggerSchema(
            Title = "Status",
            Format = "int")]
        public int Status { get; set; }
    }
}