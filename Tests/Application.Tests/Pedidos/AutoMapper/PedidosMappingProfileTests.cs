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
            var pedido = new Pedido(Guid.NewGuid(), false, 0, 150m);

            // Act
            var pedidoDto = _mapper.Map<PedidoDto>(pedido);

            // Assert
            Assert.Equal(pedido.Id, pedidoDto.Id);
            Assert.Equal(pedido.ValorTotal, pedidoDto.ValorTotal);
        }

        [Fact]
        public void DeveMapearPedidoDtoParaPedidoOutputCorretamente()
        {
            // Arrange
            var pedidoDto = new PedidoDto
            {
                Id = Guid.NewGuid(),
                ValorTotal = 150m,
                PedidoStatus = PedidoStatus.Pago
            };

            // Act
            var pedidoOutput = _mapper.Map<PedidoOutput>(pedidoDto);

            // Assert
            Assert.Equal(pedidoDto.Id, pedidoOutput.Id);
            Assert.Equal(pedidoDto.ValorTotal, pedidoOutput.ValorTotal);
        }
    }
}
