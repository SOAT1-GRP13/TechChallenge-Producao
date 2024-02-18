namespace Application.Pedidos.DTO
{
    public class PedidoItemDto
    {
        public Guid ProdutoId { get; set; }
        public string? ProdutoNome { get; set; }
        public int Quantidade { get; set; }
    }
}
