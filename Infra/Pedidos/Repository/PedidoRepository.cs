using Domain.Pedidos;
using Domain.Base.Data;
using Microsoft.EntityFrameworkCore;

namespace Infra.Pedidos.Repository
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly PedidosContext _context;

        public PedidoRepository(PedidosContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public async Task<Pedido?> ObterPorId(Guid id)
        {
            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido is null) return null;

            await _context.Entry(pedido)
                .Collection(i => i.PedidoItems).LoadAsync(); // Popula pedido evitando querys com join

            return pedido;
        }


        public void Atualizar(Pedido pedido)
        {
            _context.Pedidos.Update(pedido);
        }

        public async Task<IEnumerable<Pedido>> ObterTodosPedidos()
        {
            return await _context.Pedidos.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<Pedido>> ObterPedidosParaFila()
        {
            var pedido = await _context.Pedidos
                                       .Where(p => p.PedidoStatus != PedidoStatus.Finalizado
                                                && p.PedidoStatus != PedidoStatus.Rascunho
                                                && p.PedidoStatus != PedidoStatus.Cancelado
                                                && p.PedidoStatus != PedidoStatus.Pronto)
                                       .Include(p => p.PedidoItems)
                                       .OrderBy(p => p.DataCadastro)
                                       .OrderBy(p => p.PedidoStatus)
                                       .ToListAsync();

            return pedido;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
