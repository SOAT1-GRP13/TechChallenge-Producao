﻿using Moq;
using Domain.Pedidos;
using Application.Pedidos.DTO;
using Domain.Base.DomainObjects;
using Application.Pedidos.Commands;
using Application.Pedidos.UseCases;
using Application.Pedidos.Handlers;
using Domain.Base.Communication.Mediator;
using Domain.Base.Messages.CommonMessages.Notifications;

namespace Application.Tests.Pedidos.Handlers
{
    public class ConsultarPedidosFilaClienteCommandHandlerTests
    {
        [Fact]
        public async Task Handle_DeveRetornarPedidoDto_QuandoConsultadoComSucesso()
        {
            // Arrange
            var mediatorHandlerMock = new Mock<IMediatorHandler>();
            var pedidoUseCaseMock = new Mock<IPedidoUseCase>();

            var guid = Guid.NewGuid();

            var pedidoDto = new PedidoDto
            {
                PedidoId = guid,
                ClienteId = Guid.NewGuid(),
                PedidoStatus = PedidoStatus.EmPreparacao
            };
            var command = new ConsultarPedidosFilaClienteCommand();
            var listPedidoDto = new List<PedidoDto> { pedidoDto };

            pedidoUseCaseMock.Setup(p => p.ObterPedidosParaFilaDeExibicao()).ReturnsAsync(listPedidoDto);

            var handler = new ConsultarPedidosFilaClienteCommandHandler(mediatorHandlerMock.Object, pedidoUseCaseMock.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);
            var primeiroPedido = result.FirstOrDefault();

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(guid, primeiroPedido?.PedidoId);
            Assert.Equal(PedidoStatus.EmPreparacao, primeiroPedido?.Status);
        }

        [Fact]
        public async Task Handle_DevePublicarNotificacao_QuandoDomainExceptionLancada()
        {
            // Arrange
            var mediatorHandlerMock = new Mock<IMediatorHandler>();
            var pedidoUseCaseMock = new Mock<IPedidoUseCase>();

            var guid = Guid.NewGuid();
            var command = new ConsultarPedidosFilaClienteCommand();

            pedidoUseCaseMock.Setup(p => p.ObterPedidosParaFilaDeExibicao())
                .ThrowsAsync(new DomainException("Erro de domínio simulado"));

            var handler = new ConsultarPedidosFilaClienteCommandHandler(mediatorHandlerMock.Object, pedidoUseCaseMock.Object);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            mediatorHandlerMock.Verify(m => m.PublicarNotificacao(It.Is<DomainNotification>(dn => dn.Value == "Erro de domínio simulado")), Times.Once());
        }
    }
}
