using Domain.Pedidos;
using Domain.Base.Data;
using Microsoft.EntityFrameworkCore;

namespace Infra.Pedidos.Repository
{
    public sealed class PedidoRepository : IPedidoRepository
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

        public void Adicionar(Pedido pedido)
        {
            _context.Pedidos.Add(pedido);
        }

        public void Atualizar(Pedido pedido)
        {
            _context.Pedidos.Update(pedido);
        }

        public async Task<IEnumerable<Pedido>> ObterTodosPedidos()
        {
            return await _context.Pedidos.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<Pedido>> ObterPedidosParaFilaDeProducao()
        {
            var pedido = await _context.Pedidos
                                       .Where(p => p.PedidoStatus != PedidoStatus.Finalizado
                                                && p.PedidoStatus != PedidoStatus.Recusado
                                                && p.PedidoStatus != PedidoStatus.Pronto)
                                       .Include(p => p.PedidoItems)
                                       .OrderBy(p => p.DataCadastro)
                                       .OrderBy(p => p.PedidoStatus)
                                       .ToListAsync();

            return pedido;
        }

        public async Task<IEnumerable<Pedido>> ObterPedidosParaFilaDeExibicao()
        {
            var pedido = await _context.Pedidos
                                       .Where(p => p.PedidoStatus != PedidoStatus.Recebido
                                                && p.PedidoStatus != PedidoStatus.Finalizado
                                                && p.PedidoStatus != PedidoStatus.Recusado)
                                       .Include(p => p.PedidoItems)
                                       .OrderBy(p => p.DataCadastro)
                                       .OrderBy(p => p.PedidoStatus)
                                       .ToListAsync();

            return pedido;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            _context.Dispose();            
        }
    }
}
