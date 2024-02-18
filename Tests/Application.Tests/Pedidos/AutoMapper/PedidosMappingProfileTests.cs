using Application.Pedidos.AutoMapper;
using Application.Pedidos.Boundaries;
using Application.Pedidos.DTO;
using AutoMapper;
using Domain.Pedidos;

namespace Application.Tests.Pedidos.AutoMapper
{
    public class PedidosMappingProfileTests
    {
        private readonly IMapper _mapper;

        public PedidosMappingProfileTests()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<PedidosMappingProfile>());
            _mapper = config.CreateMapper();
        }

        [Fact]
        public void DeveMapearPedidoParaPedidoDtoCorretamente()
        {
            // Arrange
            var pedido = pedidoFake();

            // Act
            var pedidoDto = _mapper.Map<PedidoDto>(pedido);

            // Assert
            Assert.Equal(pedido.Id, pedidoDto.PedidoId);
            Assert.Equal(pedido.PedidoStatus, pedidoDto.PedidoStatus);
        }

        #region Metodos privados
        private Pedido pedidoFake()
        {
            var pedidoId = Guid.NewGuid();

            var item1 = new PedidoItem(pedidoId, Guid.NewGuid(), "Produto 1", 2);
            item1.Pedido = null;
            var item2 = new PedidoItem(pedidoId, Guid.NewGuid(), "Produto 2", 3);
            item2.Pedido = null;

            var itens = new List<PedidoItem> { item1, item2 };

            var pedido = new Pedido(Guid.NewGuid(), itens);

            return pedido;
        }
        #endregion
    }
}
