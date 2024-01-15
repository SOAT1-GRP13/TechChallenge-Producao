using Moq;
using MediatR;
using Domain.Pedidos;
using API.Controllers;
using Microsoft.AspNetCore.Mvc;
using Application.Pedidos.Commands;
using Application.Pedidos.Boundaries;
using Domain.Base.Communication.Mediator;
using Microsoft.Extensions.DependencyInjection;
using Domain.Base.Messages.CommonMessages.Notifications;

namespace API.Tests.Controllers
{
    public class PedidoControllerTests
    {
        #region Testes metodo AtualizarStatusPedido
        [Fact]
        public async Task AoAtualizarStatusPedido_DeveRetornarOk_QuandoSucesso()
        {
            // Arrange
            var serviceProvider = new ServiceCollection()
               .AddScoped<IMediatorHandler, MediatorHandler>()
               .AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>()
               .BuildServiceProvider();

            var mediatorHandlerMock = new Mock<IMediatorHandler>();
            var domainNotificationHandler = serviceProvider.GetRequiredService<INotificationHandler<DomainNotification>>();

            var input = new AtualizarStatusPedidoInput { 
                IdPedido = Guid.NewGuid(), 
                Status = 1 
            };

            var output = new PedidoOutput { 
                Id = input.IdPedido, 
                Codigo = 1,
                ValorTotal = 10,
                DataCadastro = DateTime.Now,
                PedidoStatus = (PedidoStatus)input.Status,
                MercadoPagoId = 1
            };

            mediatorHandlerMock.Setup(m => m.EnviarComando<AtualizarStatusPedidoCommand, PedidoOutput>(It.IsAny<AtualizarStatusPedidoCommand>()))
            .ReturnsAsync(output);

            var controller = new PedidoController(domainNotificationHandler, mediatorHandlerMock.Object);

            // Act
            var result = await controller.AtualizarStatusPedido(input);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<PedidoOutput>(okResult.Value);
            Assert.Equal(output.Id, returnValue.Id);
        }

        [Fact]
        public async Task AoAtualizarStatusPedido_DeveRetornarBadRequest_QuandoFalha()
        {
            // Arrange
            var mediatorHandlerMock = new Mock<IMediatorHandler>();
            var notificationHandlerMock = new Mock<INotificationHandler<DomainNotification>>();
            var notifications = new List<DomainNotification> { new DomainNotification("Error", "Erro de validação") };
            var notificationContext = new DomainNotificationHandler();

            foreach (var n in notifications)
            {
                notificationContext.Handle(n, CancellationToken.None);
            }

            var input = new AtualizarStatusPedidoInput { IdPedido = Guid.NewGuid(), Status = 1 };

            mediatorHandlerMock.Setup(m => m.EnviarComando<AtualizarStatusPedidoCommand, PedidoOutput>(It.IsAny<AtualizarStatusPedidoCommand>()))
                               .ReturnsAsync((PedidoOutput)null);
            var controller = new PedidoController(notificationContext, mediatorHandlerMock.Object);

            // Act
            var result = await controller.AtualizarStatusPedido(input);

            // Assert
            var badRequestResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
        }
        #endregion

        #region Testes metodo ConsultarStatusPedido
        [Fact]
        public async Task AoConsultarStatusPedido_DeveRetornarOk_QuandoSucesso()
        {
            // Arrange
            var serviceProvider = new ServiceCollection()
               .AddScoped<IMediatorHandler, MediatorHandler>()
               .AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>()
               .BuildServiceProvider();

            var mediatorHandlerMock = new Mock<IMediatorHandler>();
            var domainNotificationHandler = serviceProvider.GetRequiredService<INotificationHandler<DomainNotification>>();
            var pedidoId = Guid.NewGuid();
            var output = new ConsultarStatusPedidoOutput(PedidoStatus.Pago, pedidoId);

            mediatorHandlerMock.Setup(m => m.EnviarComando<ConsultarStatusPedidoCommand, ConsultarStatusPedidoOutput>(It.IsAny<ConsultarStatusPedidoCommand>()))
                .ReturnsAsync(output);

            var controller = new PedidoController(domainNotificationHandler, mediatorHandlerMock.Object);

            // Act
            var result = await controller.ConsultarStatusPedido(pedidoId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ConsultarStatusPedidoOutput>(okResult.Value);
            Assert.Equal(output.PedidoId, returnValue.PedidoId);
            Assert.Equal(output.Status, returnValue.Status);
        }

        [Fact]
        public async Task AoConsultarStatusPedido_DeveRetornarBadRequest_QuandoFalha()
        {
            // Arrange
            var mediatorHandlerMock = new Mock<IMediatorHandler>();
            var notifications = new List<DomainNotification> { new DomainNotification("Error", "Erro de validação") };
            var notificationContext = new DomainNotificationHandler();

            foreach (var n in notifications)
            {
                notificationContext.Handle(n, CancellationToken.None);
            }

            var controller = new PedidoController(notificationContext, mediatorHandlerMock.Object);

            var pedidoId = Guid.NewGuid();

            // Act
            var result = await controller.ConsultarStatusPedido(pedidoId);

            // Assert
            var badRequestResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        #endregion
    }
}
