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
using Application.Pedidos.DTO;

namespace API.Tests.Controllers
{
    public class PedidoControllerTests
    {
        #region Testes metodo PedidoEmPreparacao
        [Fact]
        public async Task AoPedidoEmPreparacao_DeveRetornarOk_QuandoSucesso()
        {
            // Arrange
            var serviceProvider = new ServiceCollection()
               .AddScoped<IMediatorHandler, MediatorHandler>()
               .AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>()
               .BuildServiceProvider();

            var mediatorHandlerMock = new Mock<IMediatorHandler>();
            var domainNotificationHandler = serviceProvider.GetRequiredService<INotificationHandler<DomainNotification>>();

            var pedidoDto = pedidoDtoFake();

            mediatorHandlerMock.Setup(m => m.EnviarComando<AtualizarStatusPedidoCommand, PedidoDto?>(It.IsAny<AtualizarStatusPedidoCommand>()))
            .ReturnsAsync(pedidoDto);

            var controller = new PedidoController(domainNotificationHandler, mediatorHandlerMock.Object);

            // Act
            var result = await controller.PedidoEmPreparacao(pedidoDto.PedidoId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<PedidoDto?>(okResult.Value);
            Assert.Equal(pedidoDto.PedidoId, returnValue?.PedidoId);
        }

        [Fact]
        public async Task AoPedidoEmPreparacao_DeveRetornarBadRequest_QuandoFalha()
        {
            // Arrange
            var mediatorHandlerMock = new Mock<IMediatorHandler>();
            var notificationHandlerMock = new Mock<INotificationHandler<DomainNotification>>();
            var notifications = new List<DomainNotification> { new DomainNotification("Error", "Erro de validação") };
            var notificationContext = new DomainNotificationHandler();

            foreach (var n in notifications)
            {
                await notificationContext.Handle(n, CancellationToken.None);
            }

            var pedidoDto = pedidoDtoFake();

            mediatorHandlerMock.Setup(m => m.EnviarComando<AtualizarStatusPedidoCommand, PedidoDto?>(It.IsAny<AtualizarStatusPedidoCommand>()));
            var controller = new PedidoController(notificationContext, mediatorHandlerMock.Object);

            // Act
            var result = await controller.PedidoEmPreparacao(pedidoDto.PedidoId);

            // Assert
            var badRequestResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
        }
        #endregion

        #region Testes metodo PedidoPronto
        [Fact]
        public async Task AoPedidoPronto_DeveRetornarOk_QuandoSucesso()
        {
            // Arrange
            var serviceProvider = new ServiceCollection()
               .AddScoped<IMediatorHandler, MediatorHandler>()
               .AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>()
               .BuildServiceProvider();

            var mediatorHandlerMock = new Mock<IMediatorHandler>();
            var domainNotificationHandler = serviceProvider.GetRequiredService<INotificationHandler<DomainNotification>>();

            var pedidoDto = pedidoDtoFake();

            mediatorHandlerMock.Setup(m => m.EnviarComando<AtualizarStatusPedidoCommand, PedidoDto?>(It.IsAny<AtualizarStatusPedidoCommand>()))
            .ReturnsAsync(pedidoDto);

            var controller = new PedidoController(domainNotificationHandler, mediatorHandlerMock.Object);

            // Act
            var result = await controller.PedidoPronto(pedidoDto.PedidoId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<PedidoDto?>(okResult.Value);
            Assert.Equal(pedidoDto.PedidoId, returnValue?.PedidoId);
        }

        [Fact]
        public async Task AoPedidoPronto_DeveRetornarBadRequest_QuandoFalha()
        {
            // Arrange
            var mediatorHandlerMock = new Mock<IMediatorHandler>();
            var notificationHandlerMock = new Mock<INotificationHandler<DomainNotification>>();
            var notifications = new List<DomainNotification> { new DomainNotification("Error", "Erro de validação") };
            var notificationContext = new DomainNotificationHandler();

            foreach (var n in notifications)
            {
                await notificationContext.Handle(n, CancellationToken.None);
            }

            var pedidoDto = pedidoDtoFake();

            mediatorHandlerMock.Setup(m => m.EnviarComando<AtualizarStatusPedidoCommand, PedidoDto?>(It.IsAny<AtualizarStatusPedidoCommand>()));
            var controller = new PedidoController(notificationContext, mediatorHandlerMock.Object);

            // Act
            var result = await controller.PedidoPronto(pedidoDto.PedidoId);

            // Assert
            var badRequestResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
        }
        #endregion

        #region Testes metodo PedidoFinalizado
        [Fact]
        public async Task AoPedidoFinalizado_DeveRetornarOk_QuandoSucesso()
        {
            // Arrange
            var serviceProvider = new ServiceCollection()
               .AddScoped<IMediatorHandler, MediatorHandler>()
               .AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>()
               .BuildServiceProvider();

            var mediatorHandlerMock = new Mock<IMediatorHandler>();
            var domainNotificationHandler = serviceProvider.GetRequiredService<INotificationHandler<DomainNotification>>();

            var pedidoDto = pedidoDtoFake();

            mediatorHandlerMock.Setup(m => m.EnviarComando<AtualizarStatusPedidoCommand, PedidoDto?>(It.IsAny<AtualizarStatusPedidoCommand>()))
            .ReturnsAsync(pedidoDto);

            var controller = new PedidoController(domainNotificationHandler, mediatorHandlerMock.Object);

            // Act
            var result = await controller.PedidoFinalizado(pedidoDto.PedidoId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<PedidoDto?>(okResult.Value);
            Assert.Equal(pedidoDto.PedidoId, returnValue?.PedidoId);
        }

        [Fact]
        public async Task AoPedidoFinalizado_DeveRetornarBadRequest_QuandoFalha()
        {
            // Arrange
            var mediatorHandlerMock = new Mock<IMediatorHandler>();
            var notificationHandlerMock = new Mock<INotificationHandler<DomainNotification>>();
            var notifications = new List<DomainNotification> { new DomainNotification("Error", "Erro de validação") };
            var notificationContext = new DomainNotificationHandler();

            foreach (var n in notifications)
            {
                await notificationContext.Handle(n, CancellationToken.None);
            }

            var pedidoDto = pedidoDtoFake();

            mediatorHandlerMock.Setup(m => m.EnviarComando<AtualizarStatusPedidoCommand, PedidoDto?>(It.IsAny<AtualizarStatusPedidoCommand>()));
            var controller = new PedidoController(notificationContext, mediatorHandlerMock.Object);

            // Act
            var result = await controller.PedidoFinalizado(pedidoDto.PedidoId);

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
            var output = new ConsultarStatusPedidoOutput(PedidoStatus.Iniciado, pedidoId);

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
                await notificationContext.Handle(n, CancellationToken.None);
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

        #region Metodos privados
        private PedidoDto pedidoDtoFake()
        {
            var item1 = new PedidoItemDto();
            item1.ProdutoId = Guid.NewGuid();
            item1.ProdutoNome = "Produto 1";
            item1.Quantidade = 2;

            var item2 = new PedidoItemDto();
            item2.ProdutoId = Guid.NewGuid();
            item2.ProdutoNome = "Produto 2";
            item2.Quantidade = 3;

            var itens = new List<PedidoItemDto> { item1, item2 };

            var pedido = new PedidoDto();
            pedido.PedidoId = Guid.NewGuid();
            pedido.ClienteId = Guid.NewGuid();
            pedido.DataCadastro = DateTime.Now;
            pedido.PedidoStatus = PedidoStatus.Iniciado;
            pedido.Items = itens;

            return pedido;
        }
        #endregion
    }
}
