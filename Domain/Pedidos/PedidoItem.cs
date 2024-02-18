using Domain.Base.DomainObjects;

namespace Domain.Pedidos
{
    public class PedidoItem : Entity
    {
        public Guid PedidoId { get; private set; }
        public Guid ProdutoId { get; private set; }
        public string ProdutoNome { get; private set; }
        public int Quantidade { get; private set; }

        // Entity Relacionamento.
        public Pedido? Pedido { get; set; }

        public PedidoItem(Guid pedidoId, Guid produtoId, string produtoNome, int quantidade)
        {
            PedidoId = pedidoId;
            ProdutoId = produtoId;
            ProdutoNome = produtoNome;
            Quantidade = quantidade;
        }
    }
}
