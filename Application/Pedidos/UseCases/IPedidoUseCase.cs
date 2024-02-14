using Domain.Pedidos;
using Application.Pedidos.DTO;

namespace Application.Pedidos.UseCases
{
    public interface IPedidoUseCase : IDisposable
    {
        Task<bool> AdicionarPedido(PedidoDto pedidoDto);
        Task<PedidoDto> TrocaStatusPedido(Guid idPedido, PedidoStatus novoStatus);
        Task<PedidoDto> ObterPedidoPorId(Guid pedidoId);
    }
}
