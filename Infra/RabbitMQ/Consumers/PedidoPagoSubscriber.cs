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
    public class PedidoPagoSubscriber : RabbitMQSubscriber
    {
        public PedidoPagoSubscriber(IServiceScopeFactory scopeFactory, IOptions<Secrets> options, IModel model) : base(options.Value.ExchangePedidoPago, options.Value.QueuePedidoPago, scopeFactory, model) { }

        protected override void InvokeCommand(PedidoDto pedidoDto, IMediatorHandler mediatorHandler)
        {
            var input = new AtualizarStatusPedidoInput(pedidoDto.PedidoId, (int)PedidoStatus.Recebido);
            var command = new AtualizarStatusPedidoCommand(input);
            mediatorHandler.EnviarComando<AtualizarStatusPedidoCommand, PedidoDto?>(command).Wait();
        }
    }
}
