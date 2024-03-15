using Moq;
using AutoMapper;
using Domain.Pedidos;
using Domain.Base.Data;
using Application.Pedidos.DTO;
using Domain.Base.DomainObjects;
using Application.Pedidos.UseCases;

namespace Application.Tests.Pedidos.UseCases
{
    public class PedidoUseCaseTests
    {
        private readonly Mock<IPedidoRepository> _pedidoRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly PedidoUseCase _pedidoUseCase;

        public PedidoUseCaseTests()
        {
            _pedidoRepositoryMock = new Mock<IPedidoRepository>();
            _mapperMock = new Mock<IMapper>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _pedidoUseCase = new PedidoUseCase(_pedidoRepositoryMock.Object, _mapperMock.Object);

            _pedidoRepositoryMock.SetupGet(r => r.UnitOfWork).Returns(_unitOfWorkMock.Object);
        }

        [Fact]
        public async Task TrocaStatusPedido_DeveRetornarPedidoAtualizado_QuandoPedidoExiste()
        {
            // Arrange
            var pedidoId = Guid.NewGuid();
            var pedido = pedidoFake();
            _pedidoRepositoryMock.Setup(r => r.ObterPorId(pedidoId)).ReturnsAsync(pedido);

            var pedidoDto = new PedidoDto();
            _mapperMock.Setup(m => m.Map<PedidoDto>(pedido)).Returns(pedidoDto);

            _unitOfWorkMock.Setup(u => u.Commit()).ReturnsAsync(true);

            // Act
            var resultado = await _pedidoUseCase.TrocaStatusPedido(pedidoId, PedidoStatus.Recebido);

            // Assert
            Assert.Equal(pedidoDto, resultado);
            _pedidoRepositoryMock.Verify(r => r.Atualizar(It.IsAny<Pedido>()), Times.Once());
            _unitOfWorkMock.Verify(u => u.Commit(), Times.Once());
        }

        [Fact]
        public async Task TrocaStatusPedido_DeveRetornarPedidoDtoVazio_QuandoPedidoNaoExiste()
        {
            // Arrange
            var pedidoId = Guid.NewGuid();
            _pedidoRepositoryMock.Setup(r => r.ObterPorId(pedidoId));

            // Act
            var resultado = await _pedidoUseCase.TrocaStatusPedido(pedidoId, PedidoStatus.Iniciado);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(Guid.Empty, resultado.PedidoId);
        }

        [Fact]
        public async Task TrocaStatusPedido_DeveLancarDomainException_QuandoStatusInvalido()
        {
            // Arrange
            var pedidoId = Guid.NewGuid();
            var pedido = pedidoFake();
            _pedidoRepositoryMock.Setup(r => r.ObterPorId(pedidoId)).ReturnsAsync(pedido);

            // Act & Assert
            await Assert.ThrowsAsync<DomainException>(() =>
                _pedidoUseCase.TrocaStatusPedido(pedidoId, PedidoStatus.Iniciado));
        }

        [Fact]
        public async Task ObterPedidoPorId_DeveRetornarPedidoDto_QuandoPedidoExiste()
        {
            // Arrange
            var pedidoId = Guid.NewGuid();
            var pedido = pedidoFake();
            _pedidoRepositoryMock.Setup(r => r.ObterPorId(pedidoId)).ReturnsAsync(pedido);

            var pedidoDto = new PedidoDto { PedidoId = pedidoId };
            _mapperMock.Setup(m => m.Map<PedidoDto>(pedido)).Returns(pedidoDto);

            // Act
            var resultado = await _pedidoUseCase.ObterPedidoPorId(pedidoId);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(pedidoId, resultado.PedidoId);
            _mapperMock.Verify(m => m.Map<PedidoDto>(pedido), Times.Once());
        }

        [Fact]
        public async Task ObterPedidoPorId_DeveRetornarNull_QuandoPedidoNaoExiste()
        {
            // Arrange
            var pedidoId = Guid.NewGuid();
            _pedidoRepositoryMock.Setup(r => r.ObterPorId(pedidoId));

            // Act
            var resultado = await _pedidoUseCase.ObterPedidoPorId(pedidoId);

            // Assert
            Assert.Null(resultado);
        }

        [Fact]
        public void Dispose_DeveChamarDisposeNoPedidoRepository()
        {
            // Act
            _pedidoUseCase.Dispose();

            // Assert
            _pedidoRepositoryMock.Verify(r => r.Dispose(), Times.Once());
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

            var pedido = new Pedido(Guid.NewGuid(), itens, string.Empty);

            return pedido;
        }
        #endregion
    }
}
