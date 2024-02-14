using Moq;
using Infra.Pedidos;
using Domain.Pedidos;
using Infra.Pedidos.Repository;
using Microsoft.EntityFrameworkCore;
using Domain.Base.Communication.Mediator;

namespace Infra.Tests.Pedidos.Repository
{
    public class PedidoRepositoryTests
    {
        [Fact]
        public async Task ObterPorId_DeveRetornarPedido_QuandoPedidoExiste()
        {
            // Arrange
            var mediatorHandler = new Mock<IMediatorHandler>();
            var options = new DbContextOptionsBuilder<PedidosContext>()
                .UseInMemoryDatabase(databaseName: "TestePedidoDb")
                .Options;

            using (var context = new PedidosContext(options, mediatorHandler.Object))
            {
                var pedidoObj = pedidoFake();
                context.Pedidos.Add(pedidoObj);
                context.SaveChanges();

                var repository = new PedidoRepository(context);

                // Act
                var pedido = await repository.ObterPorId(pedidoObj.Id);

                // Assert
                Assert.NotNull(pedido);
                Assert.Equal(pedidoObj.Id, pedido?.Id);
            }
        }

        [Fact]
        public async Task ObterPorId_DeveRetornarNull_QuandoPedidoNaoExiste()
        {
            // Arrange
            var mediatorHandler = new Mock<IMediatorHandler>();
            var options = new DbContextOptionsBuilder<PedidosContext>()
                .UseInMemoryDatabase(databaseName: "TestePedidoDb")
                .Options;

            using (var context = new PedidosContext(options, mediatorHandler.Object))
            {
                var repository = new PedidoRepository(context);

                // Act
                var pedido = await repository.ObterPorId(Guid.NewGuid());

                // Assert
                Assert.Null(pedido);
            }
        }

        #region testes Adicionar
        [Fact]
        public async Task Adicionar_DeveAdicionarPedido_QuandoPedidoEhValido()
        {
            var context = CreateDbContext();

            var pedido = pedidoFake();
            var repository = new PedidoRepository(context);

            // Ação
            repository.Adicionar(pedido);
            await context.SaveChangesAsync();

            // Assertiva
            var pedidoAdicionado = await context.Pedidos.FirstOrDefaultAsync(p => p.Id == pedido.Id);
            Assert.NotNull(pedidoAdicionado);
            Assert.Equal(pedido.ClienteId, pedidoAdicionado?.ClienteId);
        }
        #endregion

        [Fact]
        public async Task Atualizar_DeveAlterarDadosDoPedido_QuandoPedidoExiste()
        {
            // Arrange
            var mediatorHandler = new Mock<IMediatorHandler>();
            var options = new DbContextOptionsBuilder<PedidosContext>()
                .UseInMemoryDatabase(databaseName: "TestePedidoDbAtualizar")
                .Options;

            var pedido = pedidoFake();
            pedido.ColocarPedidoComoRecebido();

            // Criando e salvando o pedido original
            using (var context = new PedidosContext(options, mediatorHandler.Object))
            {                
                context.Pedidos.Add(pedido);
                await context.SaveChangesAsync();

                var repository = new PedidoRepository(context);
                var pedidoParaAtualizar = await context.Pedidos.FindAsync(pedido.Id);

                if(pedidoParaAtualizar is null)
                {
                    Assert.True(false, "Pedido não encontrado");
                    return;
                }

                pedidoParaAtualizar.ColocarPedidoEmPreparacao();
                repository.Atualizar(pedidoParaAtualizar);
                await context.SaveChangesAsync();

                var pedidoAtualizado = await context.Pedidos.FindAsync(pedido.Id);
                Assert.NotNull(pedidoAtualizado);
                Assert.Equal(PedidoStatus.EmPreparacao, pedidoAtualizado?.PedidoStatus);
            }
        }

        [Fact]
        public void Dispose_DeveChamarDisposeDoContexto()
        {
            // Arrange
            var mediatorHandler = new Mock<IMediatorHandler>();
            var options = new DbContextOptionsBuilder<PedidosContext>()
                .UseInMemoryDatabase(databaseName: "TestePedidoDbDispose")
                .Options;

            var mockContext = new Mock<PedidosContext>(options, mediatorHandler.Object);
            var repository = new PedidoRepository(mockContext.Object);

            // Act
            repository.Dispose();

            // Assert
            mockContext.Verify(x => x.Dispose(), Times.Once());
        }

        #region testes ObterTodosPedidos
        [Fact]
        public async Task ObterTodosPedidos_DeveRetornarTodosPedidos()
        {
            var context = CreateDbContext();

            var pedido1 = pedidoFake();
            var pedido2 = pedidoFake();
            context.Pedidos.Add(pedido1);
            context.Pedidos.Add(pedido2);
            await context.SaveChangesAsync();

            var repository = new PedidoRepository(context);
            var pedidos = await repository.ObterTodosPedidos();

            Assert.Contains(pedidos, p => p.Id == pedido1.Id);
            Assert.Contains(pedidos, p => p.Id == pedido2.Id);
        }
        #endregion

        #region testes ObterPedidosParaFila
        [Fact]
        public async Task ObterPedidosParaFila_DeveRetornarPedidosConformeStatus()
        {
            var context = CreateDbContext();

            var pedido1 = pedidoFake();
            pedido1.ColocarPedidoComoRecebido();
            var pedido2 = pedidoFake();
            pedido2.ColocarPedidoComoRecebido();
            pedido2.ColocarPedidoEmPreparacao();
            var pedido4 = pedidoFake();
            pedido4.CancelarPedido();
            var pedido5 = pedidoFake();
            pedido5.ColocarPedidoComoRecebido();
            pedido5.ColocarPedidoEmPreparacao();
            pedido5.ColocarPedidoComoPronto();
            context.Pedidos.AddRange(pedido1, pedido2, pedido4, pedido5);
            await context.SaveChangesAsync();

            var repository = new PedidoRepository(context);
            var pedidosParaFila = await repository.ObterPedidosParaFila();

            Assert.Contains(pedidosParaFila, p => p.Id == pedido1.Id);
            Assert.Contains(pedidosParaFila, p => p.Id == pedido2.Id);
            Assert.DoesNotContain(pedidosParaFila, p => p.Id == pedido4.Id);
            Assert.DoesNotContain(pedidosParaFila, p => p.Id == pedido5.Id);
        }
        #endregion

        #region metodos privados
        public static PedidosContext CreateDbContext()
        {
            var mediatorHandler = new Mock<IMediatorHandler>();
            var options = new DbContextOptionsBuilder<PedidosContext>()
                .UseInMemoryDatabase(databaseName: "TestePedidoDbDispose")
                .Options;

            var dbContext = new PedidosContext(options, mediatorHandler.Object);

            return dbContext;
        }

        private Pedido pedidoFake()
        {
            var item1 = new PedidoItem(Guid.NewGuid(), "Produto 1", 2);
            var item2 = new PedidoItem(Guid.NewGuid(), "Produto 2", 3);

            var itens = new List<PedidoItem> { item1, item2 };

            var pedido = new Pedido(Guid.NewGuid(), itens);

            return pedido;
        }
        #endregion

    }
}
