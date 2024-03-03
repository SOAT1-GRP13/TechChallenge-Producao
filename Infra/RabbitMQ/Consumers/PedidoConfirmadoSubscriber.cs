using RabbitMQ.Client;
using Application.Pedidos.DTO;
using Application.Pedidos.Commands;
using Application.Pedidos.Boundaries;
using Domain.Base.Communication.Mediator;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Domain.Configuration;

namespace Infra.RabbitMQ.Consumers
{
    public class PedidoConfirmadoSubscriber : RabbitMQSubscriber
    {
        public PedidoConfirmadoSubscriber(IServiceScopeFactory scopeFactory, IOptions<Secrets> options, IModel model) : base(options.Value.ExchangePedidoConfirmado, options.Value.QueuePedidoConfirmado, scopeFactory, model) { } 

        protected override void InvokeCommand(PedidoDto pedidoDto, IMediatorHandler mediatorHandler)
        {
            var input = new AdicionarPedidoInput(pedidoDto);
            var command = new AdicionarPedidoCommand(input);
            mediatorHandler.EnviarComando<AdicionarPedidoCommand, PedidoDto?>(command).Wait();
        }
    }
}
