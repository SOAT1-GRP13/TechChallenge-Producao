using AutoMapper;
using Domain.Pedidos;
using Domain.Base.DomainObjects;
using Application.Pedidos.DTO;

namespace Application.Pedidos.UseCases
{
    public class PedidoUseCase : IPedidoUseCase
    {
        #region Propriedades
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IMapper _mapper;
        #endregion

        #region Construtor
        public PedidoUseCase(
            IPedidoRepository pedidoRepository,
            IMapper mapper)
        {
            _pedidoRepository = pedidoRepository;
            _mapper = mapper;
        }
        #endregion

        #region Use Cases
        public async Task<bool> AdicionarPedido(PedidoDto pedidoDto)
        {
            var pedido = _mapper.Map<Pedido>(pedidoDto);

            if(pedido is null)
                return false;

            pedido.ColocarPedidoComoRecebido();

            _pedidoRepository.Adicionar(pedido);

            return await _pedidoRepository.UnitOfWork.Commit();
        }

        public async Task<PedidoDto> TrocaStatusPedido(Guid idPedido, PedidoStatus novoStatus)
        {
            var pedido = await _pedidoRepository.ObterPorId(idPedido);

            if (pedido is null)
                return new PedidoDto();

            pedido.AtualizarStatus(novoStatus);

            _pedidoRepository.Atualizar(pedido);

            await _pedidoRepository.UnitOfWork.Commit();

            return _mapper.Map<PedidoDto>(pedido);
        }

        public async Task<PedidoDto> ObterPedidoPorId(Guid pedidoId)
        {
            var pedido = await _pedidoRepository.ObterPorId(pedidoId);

            return _mapper.Map<PedidoDto>(pedido);
        }

        #endregion

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            _pedidoRepository.Dispose();
        }
    }
}
