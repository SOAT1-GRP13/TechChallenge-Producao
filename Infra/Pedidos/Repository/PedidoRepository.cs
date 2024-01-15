using Domain.Base.Data;
using Domain.Configuration;
using Domain.Pedidos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Infra.Pedidos.Repository
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly PedidosContext _context;
        private readonly Secrets _settings;
        private readonly DbContextOptions<PedidosContext> _optionsBuilder;

        public PedidoRepository(PedidosContext context, IOptions<Secrets> options)
        {
            _context = context;
            _settings = options.Value;
            _optionsBuilder = new DbContextOptions<PedidosContext>();
        }

        public IUnitOfWork UnitOfWork => _context;

        public async Task<Pedido> ObterPorId(Guid id)
        {
            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido == null) return null;

            await _context.Entry(pedido)
                .Collection(i => i.PedidoItems).LoadAsync(); // Popula pedido evitando querys com join

            return pedido;
        }


        public void Atualizar(Pedido pedido)
        {
            _context.Pedidos.Update(pedido);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
