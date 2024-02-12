using Domain.Base.Data;

namespace Domain.Pedidos
{
    public interface IPedidoRepository : IRepository<Pedido>
    {
        Task<Pedido?> ObterPorId(Guid id);
        void Atualizar(Pedido pedido);
        Task<IEnumerable<Pedido>> ObterTodosPedidos();
        Task<IEnumerable<Pedido>> ObterPedidosParaFila();
    }
}
