using Domain.Pedidos;
using Domain.Base.DomainObjects;

namespace Domain.Tests.Pedidos
{
    public class PedidoTests
    {
        #region CancelarPedido
        [Fact]
        public void CancelarPedido_DeveDefinirStatusComoCancelado()
        {
            // Arrange
            var pedido = pedidoFake();

            // Act
            pedido.CancelarPedido();

            // Assert
            Assert.Equal(PedidoStatus.Cancelado, pedido.PedidoStatus);
        }

        [Fact]
        public void CancelarPedido_SeJaEstiverCancelado_DeveLancarExcecao()
        {
            // Arrange
            var pedido = pedidoFake();
            pedido.CancelarPedido();

            // Act
            try 
            {
                pedido.CancelarPedido();
            }
            catch (DomainException ex)
            {
                // Assert
                Assert.Equal("Pedido já está cancelado", ex.Message);
                return;
            }

            Assert.True(false, "Deveria ter lançado exceção");
        }

        [Fact]
        public void CancelarPedido_SeJaEstiverPronto_DeveLancarExcecao()
        {
            // Arrange
            var pedido = pedidoFake();
            pedido.ColocarPedidoComoRecebido();
            pedido.ColocarPedidoEmPreparacao();
            pedido.ColocarPedidoComoPronto();

            // Act
            try
            {
                pedido.CancelarPedido();
            }
            catch (DomainException ex)
            {
                // Assert
                Assert.Equal("Pedido não pode ser cancelado, pois já está pronto", ex.Message);
                return;
            }

            Assert.True(false, "Deveria ter lançado exceção");
        }

        [Fact]
        public void CancelarPedido_SeJaEstiverEmPreparacao_DeveLancarExcecao()
        {
            // Arrange
            var pedido = pedidoFake();
            pedido.ColocarPedidoComoRecebido();
            pedido.ColocarPedidoEmPreparacao();

            // Act
            try
            {
                pedido.CancelarPedido();
            }
            catch (DomainException ex)
            {
                // Assert
                Assert.Equal("Pedido não pode ser cancelado, pois já foi para preparação", ex.Message);
                return;
            }

            Assert.True(false, "Deveria ter lançado exceção");
        }

        [Fact]
        public void CancelarPedido_SeJaEstiverFinalizado_DeveLancarExcecao()
        {
            // Arrange
            var pedido = pedidoFake();
            pedido.ColocarPedidoComoRecebido();
            pedido.ColocarPedidoEmPreparacao();
            pedido.ColocarPedidoComoPronto();
            pedido.FinalizarPedido();

            // Act
            try
            {
                pedido.CancelarPedido();
            }
            catch (DomainException ex)
            {
                // Assert
                Assert.Equal("Pedido já foi finalizado", ex.Message);
                return;
            }

            Assert.True(false, "Deveria ter lançado exceção");
        }
        #endregion

        #region ColocarPedidoComoPronto
        [Fact]
        public void ColocarPedidoComoPronto_DeveDefinirStatusComoPronto()
        {
            // Arrange
            var pedido = pedidoFake();
            pedido.ColocarPedidoComoRecebido();
            pedido.ColocarPedidoEmPreparacao();

            // Act
            pedido.ColocarPedidoComoPronto();

            // Assert
            Assert.Equal(PedidoStatus.Pronto, pedido.PedidoStatus);
        }

        [Fact]
        public void ColocarPedidoComoPronto_SeNaoEstiverEmPreparacao_DeveLancarExcecao()
        {
            // Arrange
            var pedido = pedidoFake();
            pedido.ColocarPedidoComoRecebido();

            // Act
            try
            {
                pedido.ColocarPedidoComoPronto();
            }
            catch (DomainException ex)
            {
                // Assert
                Assert.Equal("Pedido não pode ser colocado como pronto, pois o mesmo não está em preparação", ex.Message);
                return;
            }

            Assert.True(false, "Deveria ter lançado exceção");
        }
        #endregion

        #region ColocarPedidoEmPreparacao
        [Fact]
        public void ColocarPedidoEmPreparacao_DeveDefinirStatusComoEmPreparacao()
        {
            // Arrange
            var pedido = pedidoFake();
            pedido.ColocarPedidoComoRecebido();

            // Act
            pedido.ColocarPedidoEmPreparacao();

            // Assert
            Assert.Equal(PedidoStatus.EmPreparacao, pedido.PedidoStatus);
        }

        [Fact]
        public void ColocarPedidoEmPreparacao_SeNaoEstiverRecebido_DeveLancarExcecao()
        {
            // Arrange
            var pedido = pedidoFake();

            // Act
            try
            {
                pedido.ColocarPedidoEmPreparacao();
            }
            catch (DomainException ex)
            {
                // Assert
                Assert.Equal("Pedido não pode ser colocado em preparação, pois o mesmo não foi recebido", ex.Message);
                return;
            }

            Assert.True(false, "Deveria ter lançado exceção");
        }
        #endregion

        #region ColocarPedidoComoRecebido
        [Fact]
        public void ColocarPedidoComoRecebido_DeveDefinirStatusComoRecebido()
        {
            // Arrange
            var pedido = pedidoFake();

            // Act
            pedido.ColocarPedidoComoRecebido();

            // Assert
            Assert.Equal(PedidoStatus.Recebido, pedido.PedidoStatus);
        }

        [Fact]
        public void ColocarPedidoComoRecebido_SeJaEstiverCancelado_DeveLancarExcecao()
        {
            // Arrange
            var pedido = pedidoFake();
            pedido.CancelarPedido();

            // Act
            try
            {
                pedido.ColocarPedidoComoRecebido();
            }
            catch (DomainException ex)
            {
                // Assert
                Assert.Equal("Pedido já está cancelado", ex.Message);
                return;
            }

            Assert.True(false, "Deveria ter lançado exceção");
        }

        [Fact]
        public void ColocarPedidoComoRecebido_SeJaEstiverPronto_DeveLancarExcecao()
        {
            // Arrange
            var pedido = pedidoFake();
            pedido.ColocarPedidoComoRecebido();
            pedido.ColocarPedidoEmPreparacao();
            pedido.ColocarPedidoComoPronto();

            // Act
            try
            {
                pedido.ColocarPedidoComoRecebido();
            }
            catch (DomainException ex)
            {
                // Assert
                Assert.Equal("Pedido não pode ser recebido, pois já está pronto", ex.Message);
                return;
            }

            Assert.True(false, "Deveria ter lançado exceção");
        }

        [Fact]
        public void ColocarPedidoComoRecebido_SeJaEstiverEmPreparacao_DeveLancarExcecao()
        {
            // Arrange
            var pedido = pedidoFake();
            pedido.ColocarPedidoComoRecebido();
            pedido.ColocarPedidoEmPreparacao();

            // Act
            try
            {
                pedido.ColocarPedidoComoRecebido();
            }
            catch (DomainException ex)
            {
                // Assert
                Assert.Equal("Pedido não pode ser recebido, pois já foi para preparação", ex.Message);
                return;
            }

            Assert.True(false, "Deveria ter lançado exceção");
        }

        [Fact]
        public void ColocarPedidoComoRecebido_SeJaEstiverFinalizado_DeveLancarExcecao()
        {
            // Arrange
            var pedido = pedidoFake();
            pedido.ColocarPedidoComoRecebido();
            pedido.ColocarPedidoEmPreparacao();
            pedido.ColocarPedidoComoPronto();
            pedido.FinalizarPedido();

            // Act
            try
            {
                pedido.ColocarPedidoComoRecebido();
            }
            catch (DomainException ex)
            {
                // Assert
                Assert.Equal("Pedido já foi finalizado", ex.Message);
                return;
            }

            Assert.True(false, "Deveria ter lançado exceção");
        }
        #endregion

        #region FinalizarPedido
        [Fact]
        public void FinalizarPedido_DeveDefinirStatusComoFinalizado()
        {
            // Arrange
            var pedido = pedidoFake();
            pedido.ColocarPedidoComoRecebido();
            pedido.ColocarPedidoEmPreparacao();
            pedido.ColocarPedidoComoPronto();

            // Act
            pedido.FinalizarPedido();

            // Assert
            Assert.Equal(PedidoStatus.Finalizado, pedido.PedidoStatus);
        }

        [Fact]
        public void FinalizarPedido_SeNaoEstiverPronto_DeveLancarExcecao()
        {
            // Arrange
            var pedido = pedidoFake();
            pedido.ColocarPedidoComoRecebido();
            pedido.ColocarPedidoEmPreparacao();

            // Act
            try
            {
                pedido.FinalizarPedido();
            }
            catch (DomainException ex)
            {
                // Assert
                Assert.Equal("Pedido não pode ser finalizado, pois não está pronto", ex.Message);
                return;
            }

            Assert.True(false, "Deveria ter lançado exceção");
        }
        #endregion

        [Theory]
        [InlineData(PedidoStatus.Cancelado)]
        [InlineData(PedidoStatus.Pronto)]
        [InlineData(PedidoStatus.EmPreparacao)]
        [InlineData(PedidoStatus.Recebido)]
        [InlineData(PedidoStatus.Finalizado)]
        public void AtualizarStatus_DeveDefinirStatusCorreto(PedidoStatus novoStatus)
        {
            // Arrange
            var pedido = pedidoFake();

            switch (novoStatus)
            {
                case PedidoStatus.EmPreparacao:
                    pedido.ColocarPedidoComoRecebido();
                    break;
                case PedidoStatus.Pronto:
                    pedido.ColocarPedidoComoRecebido();
                    pedido.ColocarPedidoEmPreparacao();
                    break;                
                case PedidoStatus.Finalizado:
                    pedido.ColocarPedidoComoRecebido();
                    pedido.ColocarPedidoEmPreparacao();
                    pedido.ColocarPedidoComoPronto();
                    break;
                default:
                    break;
            }

            // Act
            pedido.AtualizarStatus(novoStatus);

            // Assert
            Assert.Equal(novoStatus, pedido.PedidoStatus);
        }

        [Fact]
        public void AtualizarStatus_DeveLancarExcecaoParaStatusInvalido()
        {
            // Arrange
            var pedido = pedidoFake();
            var statusInvalido = (PedidoStatus)999; // Um valor que não existe no enum

            // Act & Assert
            var exception = Assert.Throws<DomainException>(() => pedido.AtualizarStatus(statusInvalido));
            Assert.Equal("Status do pedido inválido", exception.Message);
        }

        #region Metodos privados
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
