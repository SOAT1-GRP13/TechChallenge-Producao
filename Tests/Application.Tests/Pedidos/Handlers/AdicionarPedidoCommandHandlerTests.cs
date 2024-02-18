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
    public class AdicionarPedidoPedidoCommandHandlerTests
    {
        [Fact]
        public async Task Handle_DeveRetornarPedidoOutput_QuandoPedidoAtualizadoComSucesso()
        {
            // Arrange
            var mediatorHandlerMock = new Mock<IMediatorHandler>();
            var pedidoUseCaseMock = new Mock<IPedidoUseCase>();
            var mapperMock = new Mock<IMapper>();

            var guid = Guid.NewGuid();

            var pedidoDto = new PedidoDto
            {
                PedidoId = guid,
                ClienteId = Guid.NewGuid(),
                DataCadastro = DateTime.Now,
                PedidoStatus = PedidoStatus.Iniciado,
            };
            var command = new AdicionarPedidoCommand(new AdicionarPedidoInput(pedidoDto));            

            pedidoUseCaseMock.Setup(p => p.AdicionarPedido(pedidoDto)).ReturnsAsync(true);

            var handler = new AdicionarPedidoCommandHandler(pedidoUseCaseMock.Object, mediatorHandlerMock.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(command.Input.pedidoDto.PedidoId, result?.PedidoId);
            Assert.Equal(PedidoStatus.Iniciado, result?.PedidoStatus);
        }

        [Fact]
        public async Task Handle_DevePublicarNotificacao_QuandoPedidoInvalido()
        {
            // Arrange
            var mediatorHandlerMock = new Mock<IMediatorHandler>();
            var pedidoUseCaseMock = new Mock<IPedidoUseCase>();
            var pedidoDto = new PedidoDto();
            var command = new AdicionarPedidoCommand(new AdicionarPedidoInput(pedidoDto)); // Dados inválidos
            var handler = new AdicionarPedidoCommandHandler(pedidoUseCaseMock.Object, mediatorHandlerMock.Object);

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
            var guid = Guid.NewGuid();
            var pedidoDto = new PedidoDto
            {
                PedidoId = guid,
                ClienteId = Guid.NewGuid(),
                DataCadastro = DateTime.Now,
                PedidoStatus = PedidoStatus.Iniciado,
            };
            var command = new AdicionarPedidoCommand(new AdicionarPedidoInput(pedidoDto));

            pedidoUseCaseMock.Setup(p => p.AdicionarPedido(pedidoDto))
                .ThrowsAsync(new DomainException("Erro de domínio simulado"));

            var handler = new AdicionarPedidoCommandHandler(pedidoUseCaseMock.Object, mediatorHandlerMock.Object);

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
            var guid = Guid.NewGuid();
            var pedidoDto = new PedidoDto
            {
                PedidoId = guid,
                ClienteId = Guid.NewGuid(),
                DataCadastro = DateTime.Now,
                PedidoStatus = PedidoStatus.Iniciado,
            };
            var command = new AdicionarPedidoCommand(new AdicionarPedidoInput(pedidoDto));
            var notificationHandler = new DomainNotificationHandler();

            pedidoUseCaseMock.Setup(p => p.AdicionarPedido(pedidoDto))
                .ReturnsAsync(false); // Pedido não encontrado

            var handler = new AdicionarPedidoCommandHandler(pedidoUseCaseMock.Object, mediatorHandlerMock.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            mediatorHandlerMock.Verify(m => m.PublicarNotificacao(It.IsAny<DomainNotification>()), Times.AtLeastOnce());
        }
    }
}
