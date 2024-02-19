using RabbitMQ.Client;
using Application.Pedidos.DTO;
using Application.Pedidos.Commands;
using Application.Pedidos.Boundaries;
using Domain.Base.Communication.Mediator;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.RabbitMQ.Consumers
{
    public class PedidoConfirmadoSubscriber : RabbitMQSubscriber
    {
        public PedidoConfirmadoSubscriber( IServiceScopeFactory scopeFactory, RabbitMQOptions options, IModel model) : base(scopeFactory, options.QueuePedidoConfirmado, model) { } 

        protected override void InvokeCommand(PedidoDto pedidoDto, IMediatorHandler mediatorHandler)
        {
            var input = new AdicionarPedidoInput(pedidoDto);
            var command = new AdicionarPedidoCommand(input);
            mediatorHandler.EnviarComando<AdicionarPedidoCommand, PedidoDto?>(command).Wait();
        }
    }
}
