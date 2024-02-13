using Moq;
using Domain.Pedidos;
using Application.Pedidos.DTO;
using Domain.Base.DomainObjects;
using Application.Pedidos.Handlers;
using Application.Pedidos.UseCases;
using Application.Pedidos.Commands;
using Domain.Base.Communication.Mediator;
using Domain.Base.Messages.CommonMessages.Notifications;

namespace Application.Tests.Pedidos.Handlers
{
    public class ConsultarStatusPedidoCommandHandlerTests
    {
        [Fact]
        public async Task Handle_DeveRetornarPedidoOutput_QuandoPedidoAtualizadoComSucesso()
        {
            // Arrange
            var mediatorHandlerMock = new Mock<IMediatorHandler>();
            var pedidoUseCaseMock = new Mock<IPedidoUseCase>();

            var guid = Guid.NewGuid();

            var command = new ConsultarStatusPedidoCommand(guid);
            var pedidoDto = new PedidoDto
            {
                PedidoId = guid,
                ClienteId = Guid.NewGuid(),
                DataCadastro = DateTime.Now,
                PedidoStatus = PedidoStatus.Iniciado,
            };

            // Correção aqui: configurando corretamente o mock para o método ObterPedidoPorId
            pedidoUseCaseMock.Setup(p => p.ObterPedidoPorId(guid)).ReturnsAsync(pedidoDto);

            var handler = new ConsultarStatusPedidoCommandHandler(mediatorHandlerMock.Object, pedidoUseCaseMock.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(guid, result.PedidoId);
            Assert.Equal(PedidoStatus.Iniciado, result.Status);
        }

        [Fact]
        public async Task Handle_DevePublicarNotificacao_QuandoPedidoInvalido()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var mediatorHandlerMock = new Mock<IMediatorHandler>();
            var pedidoUseCaseMock = new Mock<IPedidoUseCase>();
            var command = new ConsultarStatusPedidoCommand(Guid.Empty); // Dados inválidos
            var handler = new ConsultarStatusPedidoCommandHandler(mediatorHandlerMock.Object, pedidoUseCaseMock.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            mediatorHandlerMock.Verify(m => m.PublicarNotificacao(It.IsAny<DomainNotification>()), Times.AtLeastOnce());
            Assert.False(command.EhValido()); // Garante que o comando é inválido
        }

        [Fact]
        public async Task Handle_DevePublicarNotificacao_QuandoDomainExceptionLancada()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var mediatorHandlerMock = new Mock<IMediatorHandler>();
            var pedidoUseCaseMock = new Mock<IPedidoUseCase>();
            var command = new ConsultarStatusPedidoCommand(guid);

            pedidoUseCaseMock.Setup(p => p.ObterPedidoPorId(guid))
                .ThrowsAsync(new DomainException("Erro de domínio simulado"));

            var handler = new ConsultarStatusPedidoCommandHandler(mediatorHandlerMock.Object, pedidoUseCaseMock.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            mediatorHandlerMock.Verify(m => m.PublicarNotificacao(It.Is<DomainNotification>(dn => dn.Value == "Erro de domínio simulado")), Times.Once());
        }
    }
}
