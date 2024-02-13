using Moq;
using AutoMapper;
using Domain.Pedidos;
using Application.Pedidos.DTO;
using Domain.Base.DomainObjects;
using Application.Pedidos.Commands;
using Application.Pedidos.Handlers;
using Application.Pedidos.UseCases;
using Application.Pedidos.Boundaries;
using Domain.Base.Communication.Mediator;
using Domain.Base.Messages.CommonMessages.Notifications;


namespace Application.Tests.Pedidos.Handlers
{
    public class AtualizarStatusPedidoCommandHandlerTests
    {
        [Fact]
        public async Task Handle_DeveRetornarPedidoOutput_QuandoPedidoAtualizadoComSucesso()
        {
            // Arrange
            var mediatorHandlerMock = new Mock<IMediatorHandler>();
            var pedidoUseCaseMock = new Mock<IPedidoUseCase>();
            var mapperMock = new Mock<IMapper>();

            var guid = Guid.NewGuid();

            var command = new AtualizarStatusPedidoCommand(new AtualizarStatusPedidoInput( guid, 1));
            var pedidoDto = new PedidoDto {
                PedidoId = guid,
                ClienteId = Guid.NewGuid(),
                DataCadastro = DateTime.Now,
                PedidoStatus = PedidoStatus.Iniciado,
            };

            pedidoUseCaseMock.Setup(p => p.TrocaStatusPedido(command.Input.IdPedido, (PedidoStatus)command.Input.Status)).ReturnsAsync(pedidoDto);

            var handler = new AtualizarStatusPedidoCommandHandler(pedidoUseCaseMock.Object, mediatorHandlerMock.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(command.Input.IdPedido, result?.PedidoId);
            Assert.Equal(PedidoStatus.Iniciado, result?.PedidoStatus);
        }

        [Fact]
        public async Task Handle_DevePublicarNotificacao_QuandoPedidoInvalido()
        {
            // Arrange
            var mediatorHandlerMock = new Mock<IMediatorHandler>();
            var pedidoUseCaseMock = new Mock<IPedidoUseCase>();
            var command = new AtualizarStatusPedidoCommand(new AtualizarStatusPedidoInput(Guid.Empty,-1 )); // Dados inválidos
            var handler = new AtualizarStatusPedidoCommandHandler(pedidoUseCaseMock.Object, mediatorHandlerMock.Object);

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
            var mediatorHandlerMock = new Mock<IMediatorHandler>();
            var pedidoUseCaseMock = new Mock<IPedidoUseCase>();
            var command = new AtualizarStatusPedidoCommand(new AtualizarStatusPedidoInput(Guid.NewGuid(), 1));

            pedidoUseCaseMock.Setup(p => p.TrocaStatusPedido(command.Input.IdPedido, (PedidoStatus)command.Input.Status))
                .ThrowsAsync(new DomainException("Erro de domínio simulado"));

            var handler = new AtualizarStatusPedidoCommandHandler(pedidoUseCaseMock.Object, mediatorHandlerMock.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            mediatorHandlerMock.Verify(m => m.PublicarNotificacao(It.Is<DomainNotification>(dn => dn.Value == "Erro de domínio simulado")), Times.Once());
        }

        [Fact]
        public async Task Handle_DevePublicarNotificacao_QuandoPedidoNaoEncontrado()
        {
            // Arrange
            var mediatorHandlerMock = new Mock<IMediatorHandler>();
            var pedidoUseCaseMock = new Mock<IPedidoUseCase>();
            var command = new AtualizarStatusPedidoCommand(new AtualizarStatusPedidoInput(Guid.NewGuid(), 1));
            var notificationHandler = new DomainNotificationHandler();

            pedidoUseCaseMock.Setup(p => p.TrocaStatusPedido(command.Input.IdPedido, (PedidoStatus)command.Input.Status))
                .ReturnsAsync(new PedidoDto()); // Pedido não encontrado

            var handler = new AtualizarStatusPedidoCommandHandler(pedidoUseCaseMock.Object, mediatorHandlerMock.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            mediatorHandlerMock.Verify(m => m.PublicarNotificacao(It.IsAny<DomainNotification>()), Times.AtLeastOnce());
        }
    }
}
