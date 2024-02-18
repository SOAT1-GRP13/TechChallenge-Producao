using Domain.Base.DomainObjects;

namespace Domain.Pedidos
{
    public class PedidoItem : Entity
    {
        public Guid PedidoId { get; set; }
        public Guid ProdutoId { get; set; }
        public string ProdutoNome { get; set; }
        public int Quantidade { get; set; }

        // Entity Relacionamento.
        public Pedido? Pedido { get; set; }

        public PedidoItem() { 
            PedidoId = Guid.Empty;
            ProdutoId = Guid.Empty;
            ProdutoNome = string.Empty;
        }

        public PedidoItem(Guid pedidoId, Guid produtoId, string produtoNome, int quantidade)
        {
            PedidoId = pedidoId;
            ProdutoId = produtoId;
            ProdutoNome = produtoNome;
            Quantidade = quantidade;
        }
    }
}
