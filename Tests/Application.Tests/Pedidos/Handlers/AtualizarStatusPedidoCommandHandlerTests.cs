using Application.Pedidos.Boundaries;
using Application.Pedidos.Commands;
using Application.Pedidos.DTO;
using Application.Pedidos.Handlers;
using Application.Pedidos.UseCases;
using AutoMapper;
using Domain.Base.Communication.Mediator;
using Domain.Base.DomainObjects;
using Domain.Base.Messages.CommonMessages.Notifications;
using Domain.Pedidos;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;

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

            var command = new AtualizarStatusPedidoCommand(new AtualizarStatusPedidoInput { IdPedido = guid, Status = 1 });
            var pedidoDto = new PedidoDto {
                Id = guid,
                Codigo = 1,
                ValorTotal = 10,
                DataCadastro = DateTime.Now,
                PedidoStatus = PedidoStatus.Pago,
            };

            var pedidoOutput = new PedidoOutput
            {
                Id = guid,
                Codigo = 1,
                ValorTotal = 10,
                DataCadastro = DateTime.Now,
                PedidoStatus = PedidoStatus.Pago,
                MercadoPagoId = 1
            };

            pedidoUseCaseMock.Setup(p => p.TrocaStatusPedido(command.Input.IdPedido, (PedidoStatus)command.Input.Status)).ReturnsAsync(pedidoDto);
            mapperMock.Setup(m => m.Map<PedidoOutput>(It.IsAny<PedidoDto>())).Returns(pedidoOutput);

            var handler = new AtualizarStatusPedidoCommandHandler(pedidoUseCaseMock.Object, mediatorHandlerMock.Object, mapperMock.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(command.Input.IdPedido, result.Id);
            Assert.Equal(PedidoStatus.Pago, result.PedidoStatus);
        }

        [Fact]
        public async Task Handle_DevePublicarNotificacao_QuandoPedidoInvalido()
        {
            // Arrange
            var mediatorHandlerMock = new Mock<IMediatorHandler>();
            var command = new AtualizarStatusPedidoCommand(new AtualizarStatusPedidoInput { IdPedido = Guid.Empty, Status = -1 }); // Dados inválidos
            var handler = new AtualizarStatusPedidoCommandHandler(null, mediatorHandlerMock.Object, null); // As outras dependências não são necessárias para este teste

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
            var command = new AtualizarStatusPedidoCommand(new AtualizarStatusPedidoInput { IdPedido = Guid.NewGuid(), Status = 1 });

            pedidoUseCaseMock.Setup(p => p.TrocaStatusPedido(command.Input.IdPedido, (PedidoStatus)command.Input.Status))
                .ThrowsAsync(new DomainException("Erro de domínio simulado"));

            var handler = new AtualizarStatusPedidoCommandHandler(pedidoUseCaseMock.Object, mediatorHandlerMock.Object, null);

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
            var mapperMock = new Mock<IMapper>();
            var command = new AtualizarStatusPedidoCommand(new AtualizarStatusPedidoInput { IdPedido = Guid.NewGuid(), Status = 1 });
            var notificationHandler = new DomainNotificationHandler();

            pedidoUseCaseMock.Setup(p => p.TrocaStatusPedido(command.Input.IdPedido, (PedidoStatus)command.Input.Status))
                .ReturnsAsync(new PedidoDto()); // Pedido não encontrado

            var handler = new AtualizarStatusPedidoCommandHandler(pedidoUseCaseMock.Object, mediatorHandlerMock.Object, mapperMock.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            mediatorHandlerMock.Verify(m => m.PublicarNotificacao(It.IsAny<DomainNotification>()), Times.AtLeastOnce());
        }
    }
}
