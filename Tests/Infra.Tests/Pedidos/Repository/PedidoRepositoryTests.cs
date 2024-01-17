using Moq;
using Infra.Pedidos;
using Domain.Pedidos;
using Infra.Pedidos.Repository;
using Microsoft.EntityFrameworkCore;

namespace Infra.Tests.Pedidos.Repository
{
    public class PedidoRepositoryTests
    {
        [Fact]
        public async Task ObterPorId_DeveRetornarPedido_QuandoPedidoExiste()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<PedidosContext>()
                .UseInMemoryDatabase(databaseName: "TestePedidoDb")
                .Options;

            using (var context = new PedidosContext(options, null))
            {
                var pedidoObj = new Pedido(Guid.NewGuid(), false, 0, 10);
                context.Pedidos.Add(pedidoObj);
                context.SaveChanges();

                var repository = new PedidoRepository(context, null);

                // Act
                var pedido = await repository.ObterPorId(pedidoObj.Id);

                // Assert
                Assert.NotNull(pedido);
                Assert.Equal(pedidoObj.Id, pedido.Id);
            }
        }

        [Fact]
        public async Task ObterPorId_DeveRetornarNull_QuandoPedidoNaoExiste()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<PedidosContext>()
                .UseInMemoryDatabase(databaseName: "TestePedidoDb")
                .Options;

            using (var context = new PedidosContext(options, null))
            {
                var repository = new PedidoRepository(context, null);

                // Act
                var pedido = await repository.ObterPorId(Guid.NewGuid());

                // Assert
                Assert.Null(pedido);
            }
        }

        [Fact]
        public async Task Atualizar_DeveAlterarDadosDoPedido_QuandoPedidoExiste()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<PedidosContext>()
                .UseInMemoryDatabase(databaseName: "TestePedidoDbAtualizar")
                .Options;

            var pedido = new Pedido(Guid.NewGuid(), false, 0, 10);
            pedido.TornarRascunho();

            // Criando e salvando o pedido original
            using (var context = new PedidosContext(options, null))
            {                
                context.Pedidos.Add(pedido);
                await context.SaveChangesAsync();

                var repository = new PedidoRepository(context, null);
                var pedidoParaAtualizar = await context.Pedidos.FindAsync(pedido.Id);
                pedidoParaAtualizar.IniciarPedido();
                repository.Atualizar(pedidoParaAtualizar);
                await context.SaveChangesAsync();

                var pedidoAtualizado = await context.Pedidos.FindAsync(pedido.Id);
                Assert.NotNull(pedidoAtualizado);
                Assert.Equal(PedidoStatus.Iniciado, pedido.PedidoStatus);
            }
        }

        [Fact]
        public void Dispose_DeveChamarDisposeDoContexto()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<PedidosContext>()
                .UseInMemoryDatabase(databaseName: "TestePedidoDbDispose")
                .Options;

            var mockContext = new Mock<PedidosContext>(options, null);
            var repository = new PedidoRepository(mockContext.Object, null);

            // Act
            repository.Dispose();

            // Assert
            mockContext.Verify(x => x.Dispose(), Times.Once());
        }
    }
}
