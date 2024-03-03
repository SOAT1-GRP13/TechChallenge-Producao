using Moq;
using Domain.Pedidos;
using Domain.RabbitMQ;
using Application.Pedidos.DTO;
using Domain.Base.DomainObjects;
using Application.Pedidos.Commands;
using Application.Pedidos.UseCases;
using Application.Pedidos.Handlers;
using Application.Pedidos.Boundaries;
using Microsoft.Extensions.Configuration;
using Domain.Base.Communication.Mediator;
using Domain.Base.Messages.CommonMessages.Notifications;
using Microsoft.Extensions.Options;
using Domain.Configuration;

namespace Application.Tests.Pedidos.Handlers
{
    public class PedidoProntoCommandHandlerTests
    {
        private readonly Mock<IMediatorHandler> _mediatorHandlerMock;
        private readonly Mock<IRabbitMQService> _rabbitMQServiceMock;
        private readonly Mock<IOptions<Secrets>> _mockOptions;
        private readonly Secrets _secrets;
        private readonly Mock<IPedidoUseCase> _pedidoUseCaseMock;
        private readonly PedidoProntoCommandHandler _handler;
        public PedidoProntoCommandHandlerTests()
        {
            _mockOptions = new Mock<IOptions<Secrets>>();
            _secrets = new Secrets()
            {
                ExchangePedidoPronto = "exc_pedido_pronto"

            };
            _pedidoUseCaseMock = new Mock<IPedidoUseCase>();
            _mockOptions.Setup(opt => opt.Value).Returns(_secrets);
            _rabbitMQServiceMock = new Mock<IRabbitMQService>();
            _mediatorHandlerMock = new Mock<IMediatorHandler>();
            _handler = new PedidoProntoCommandHandler(
                _pedidoUseCaseMock.Object,
                _mediatorHandlerMock.Object,
                _rabbitMQServiceMock.Object,
                _mockOptions.Object
                );
        }
        [Fact]
        public async Task Handle_DeveRetornarPedidoDto_QuandoPedidoAtualizadoComSucesso()
        {
            // Arrange
            var guid = Guid.NewGuid();

            var pedidoDto = new PedidoDto
            {
                PedidoId = guid,
                PedidoStatus = PedidoStatus.Pronto
            };
            var command = new PedidoProntoCommand(new AtualizarStatusPedidoInput(guid, (int)PedidoStatus.Pronto));

            _pedidoUseCaseMock.Setup(p => p.TrocaStatusPedido(guid, PedidoStatus.Pronto)).ReturnsAsync(pedidoDto);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(guid, result?.PedidoId);
            Assert.Equal(PedidoStatus.Pronto, result?.PedidoStatus);

            _rabbitMQServiceMock.Verify(r => r.PublicaMensagem(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [Fact]
        public async Task Handle_DevePublicarNotificacao_QuandoPedidoInvalido()
        {
            // Arrange
            // Criando um comando com ID de pedido inválido (Guid vazio)
            var command = new PedidoProntoCommand(new AtualizarStatusPedidoInput(Guid.Empty, (int)PedidoStatus.Pronto));
            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mediatorHandlerMock.Verify(m => m.PublicarNotificacao(It.IsAny<DomainNotification>()), Times.AtLeastOnce());
            Assert.False(command.EhValido());
        }

        [Fact]
        public async Task Handle_DevePublicarNotificacao_QuandoDomainExceptionLancada()
        {
            // Arrange
            var guid = Guid.NewGuid();
            var command = new PedidoProntoCommand(new AtualizarStatusPedidoInput(guid, (int)PedidoStatus.Pronto));

            _pedidoUseCaseMock.Setup(p => p.TrocaStatusPedido(guid, PedidoStatus.Pronto))
                .ThrowsAsync(new DomainException("Erro de domínio simulado"));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mediatorHandlerMock.Verify(m => m.PublicarNotificacao(It.Is<DomainNotification>(dn => dn.Value == "Erro de domínio simulado")), Times.Once());
        }

        [Fact]
        public async Task Handle_DevePublicarNotificacaoERetornarNull_QuandoPedidoNaoEncontrado()
        {
            // Arrange;
            var guid = Guid.NewGuid();
            var command = new PedidoProntoCommand(new AtualizarStatusPedidoInput(guid, (int)PedidoStatus.Pronto));

            // Simulando um pedido não encontrado ao retornar null
            _pedidoUseCaseMock.Setup(p => p.TrocaStatusPedido(guid, PedidoStatus.Pronto));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            // Verifica se uma notificação com a mensagem "Pedido não encontrado" foi publicada
            _mediatorHandlerMock.Verify(m => m.PublicarNotificacao(It.Is<DomainNotification>(dn => dn.Value == "Pedido não encontrado")), Times.Once());

            // Verifica se o resultado é null
            Assert.Null(result);
        }
    }
}
