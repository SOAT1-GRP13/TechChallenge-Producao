using Domain.Pedidos;
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
    public class PedidoRecusadoSubscriber : RabbitMQSubscriber
    {
        public PedidoRecusadoSubscriber(IServiceScopeFactory scopeFactory, IOptions<Secrets> options, IModel model) : base(options.Value.ExchangePedidoRecusado, options.Value.QueuePedidoRecusado, scopeFactory, model) { }

        protected override void InvokeCommand(PedidoDto pedidoDto, IMediatorHandler mediatorHandler)
        {
            var input = new AtualizarStatusPedidoInput(pedidoDto.PedidoId, (int)PedidoStatus.Recusado);
            var command = new AtualizarStatusPedidoCommand(input);
            mediatorHandler.EnviarComando<AtualizarStatusPedidoCommand, PedidoDto?>(command).Wait();
        }
    }
}
