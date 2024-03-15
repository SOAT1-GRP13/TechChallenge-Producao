using Domain.Pedidos;

namespace Application.Pedidos.DTO
{
    public class PedidoDto
    {
        public Guid PedidoId { get; set; }
        public Guid ClienteId { get; set; }
        public DateTime DataCadastro { get; set; }
        public PedidoStatus PedidoStatus { get; set; }
        public string ClienteEmail { get; set; } = string.Empty;

        public List<PedidoItemDto> Items { get; set; } = new List<PedidoItemDto>();
    }
}
