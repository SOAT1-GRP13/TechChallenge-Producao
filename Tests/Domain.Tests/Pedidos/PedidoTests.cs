using Domain.Base.DomainObjects;
using Domain.Pedidos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Tests.Pedidos
{
    public class PedidoTests
    {
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
        public void ColocarPedidoComoPronto_DeveDefinirStatusComoPronto()
        {
            // Arrange
            var pedido = pedidoFake();

            // Act
            pedido.ColocarPedidoComoPronto();

            // Assert
            Assert.Equal(PedidoStatus.Pronto, pedido.PedidoStatus);
        }

        [Fact]
        public void ColocarPedidoEmPreparacao_DeveDefinirStatusComoPago()
        {
            // Arrange
            var pedido = pedidoFake();

            // Act
            pedido.ColocarPedidoEmPreparacao();

            // Assert
            Assert.Equal(PedidoStatus.EmPreparacao, pedido.PedidoStatus);
        }

        [Fact]
        public void ColocarPedidoComoRecebido_DeveDefinirStatusComoPago()
        {
            // Arrange
            var pedido = pedidoFake();

            // Act
            pedido.ColocarPedidoComoRecebido();

            // Assert
            Assert.Equal(PedidoStatus.Recebido, pedido.PedidoStatus);
        }

        [Theory]
        [InlineData(PedidoStatus.Rascunho)]
        [InlineData(PedidoStatus.Iniciado)]
        [InlineData(PedidoStatus.Pago)]
        [InlineData(PedidoStatus.Cancelado)]
        [InlineData(PedidoStatus.Pronto)]
        [InlineData(PedidoStatus.EmPreparacao)]
        [InlineData(PedidoStatus.Recebido)]
        [InlineData(PedidoStatus.Finalizado)]
        public void AtualizarStatus_DeveDefinirStatusCorreto(PedidoStatus novoStatus)
        {
            // Arrange
            var pedido = pedidoFake();

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
