using Application.Pedidos.DTO;
using Application.Pedidos.UseCases;
using AutoMapper;
using Domain.Base.Data;
using Domain.Base.DomainObjects;
using Domain.Pedidos;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var pedido = new Pedido(pedidoId, false, 0, 100);
            _pedidoRepositoryMock.Setup(r => r.ObterPorId(pedidoId)).ReturnsAsync(pedido);

            var pedidoDto = new PedidoDto();
            _mapperMock.Setup(m => m.Map<PedidoDto>(pedido)).Returns(pedidoDto);

            _unitOfWorkMock.Setup(u => u.Commit()).ReturnsAsync(true);

            // Act
            var resultado = await _pedidoUseCase.TrocaStatusPedido(pedidoId, PedidoStatus.Pago);

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
            _pedidoRepositoryMock.Setup(r => r.ObterPorId(pedidoId)).ReturnsAsync((Pedido)null);

            // Act
            var resultado = await _pedidoUseCase.TrocaStatusPedido(pedidoId, PedidoStatus.Pago);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(Guid.Empty, resultado.Id);
        }

        [Fact]
        public async Task TrocaStatusPedido_DeveLancarDomainException_QuandoStatusInvalido()
        {
            // Arrange
            var pedidoId = Guid.NewGuid();
            var pedido = new Pedido(pedidoId, false, 0, 100);
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
            var pedido = new Pedido(pedidoId, false, 0, 100);
            _pedidoRepositoryMock.Setup(r => r.ObterPorId(pedidoId)).ReturnsAsync(pedido);

            var pedidoDto = new PedidoDto { Id = pedidoId };
            _mapperMock.Setup(m => m.Map<PedidoDto>(pedido)).Returns(pedidoDto);

            // Act
            var resultado = await _pedidoUseCase.ObterPedidoPorId(pedidoId);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(pedidoId, resultado.Id);
            _mapperMock.Verify(m => m.Map<PedidoDto>(pedido), Times.Once());
        }

        [Fact]
        public async Task ObterPedidoPorId_DeveRetornarNull_QuandoPedidoNaoExiste()
        {
            // Arrange
            var pedidoId = Guid.NewGuid();
            _pedidoRepositoryMock.Setup(r => r.ObterPorId(pedidoId)).ReturnsAsync((Pedido)null);

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
    }
}
