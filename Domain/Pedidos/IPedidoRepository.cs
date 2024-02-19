using Domain.Base.Data;

namespace Domain.Pedidos
{
    public interface IPedidoRepository : IRepository
    {
        Task<Pedido?> ObterPorId(Guid id);
        void Adicionar(Pedido pedido);
        void Atualizar(Pedido pedido);
        Task<IEnumerable<Pedido>> ObterTodosPedidos();
        Task<IEnumerable<Pedido>> ObterPedidosParaFilaDeProducao();
        Task<IEnumerable<Pedido>> ObterPedidosParaFilaDeExibicao();
    }
}
