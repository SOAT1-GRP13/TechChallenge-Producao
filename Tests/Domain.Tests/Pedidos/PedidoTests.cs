﻿using Domain.Pedidos;
using Domain.Base.DomainObjects;

namespace Domain.Tests.Pedidos
{
    public class PedidoTests
    {
        #region IniciarPedido
        [Fact]
        public void IniciarPedido_DeveDefinirStatusComoIniciado()
        {
            // Arrange
            var pedido = pedidoFake();

            // Act
            pedido.IniciarPedido();

            // Assert
            Assert.Equal(PedidoStatus.Iniciado, pedido.PedidoStatus);
        }

        [Fact]
        public void IniciarPedido_SeJaEstiverIniciado_DeveLancarExcecao()
        {
            // Arrange
            var pedido = pedidoFake();
            pedido.IniciarPedido();
            pedido.ColocarPedidoComoRecebido();

            // Act
            try
            {
                pedido.IniciarPedido();
            }
            catch (DomainException ex)
            {
                // Assert
                Assert.Equal("Pedido já possui outro status", ex.Message);
                return;
            }

            Assert.True(false, "Deveria ter lançado exceção");
        }
        #endregion

        #region RecusarPedido
        [Fact]
        public void RecusarPedido_DeveDefinirStatusComoRecusado()
        {
            // Arrange
            var pedido = pedidoFake();
            pedido.IniciarPedido();

            // Act
            pedido.RecusarPedido();

            // Assert
            Assert.Equal(PedidoStatus.Recusado, pedido.PedidoStatus);
        }

        [Fact]
        public void RecusarPedido_SeNaoEstiverRecebido_DeveLancarExcecao()
        {
            // Arrange
            var pedido = pedidoFake();

            // Act
            try
            {
                pedido.RecusarPedido();
            }
            catch (DomainException ex)
            {
                // Assert
                Assert.Equal("Pedido não pode ser recusado, pois não está com status de recebido", ex.Message);
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
        public void ColocarPedidoComoRecebido_SeJaEstiverRecusado_DeveLancarExcecao()
        {
            // Arrange
            var pedido = pedidoFake();
            pedido.IniciarPedido();
            pedido.RecusarPedido();

            // Act
            try
            {
                pedido.ColocarPedidoComoRecebido();
            }
            catch (DomainException ex)
            {
                // Assert
                Assert.Equal("Pedido não pode ser recebido, pois já foi recusado", ex.Message);
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
        [InlineData(PedidoStatus.Pronto)]
        [InlineData(PedidoStatus.EmPreparacao)]
        [InlineData(PedidoStatus.Recebido)]
        [InlineData(PedidoStatus.Finalizado)]
        [InlineData(PedidoStatus.Recusado)]
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
                case PedidoStatus.Recusado:
                    pedido.IniciarPedido();
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
