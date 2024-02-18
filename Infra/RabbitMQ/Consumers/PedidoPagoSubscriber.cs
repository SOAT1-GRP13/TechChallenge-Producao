using System.Text;
using Domain.Pedidos;
using RabbitMQ.Client;
using System.Text.Json;
using RabbitMQ.Client.Events;
using Application.Pedidos.DTO;
using Application.Pedidos.Commands;
using Application.Pedidos.Boundaries;
using Domain.Base.Communication.Mediator;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.RabbitMQ.Consumers
{
    public class PedidoPagoSubscriber : RabbitMQSubscriber
    {
        public PedidoPagoSubscriber(IServiceScopeFactory scopeFactory, RabbitMQOptions options, IModel model) : base(scopeFactory, options.QueuePedidoPago, model) { }

        protected override void InvokeReceivedEvent(object? model, BasicDeliverEventArgs ea)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var mediatorHandler = scope.ServiceProvider.GetRequiredService<IMediatorHandler>();

                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                PedidoDto pedidoPago;
                try
                {
                    pedidoPago = JsonSerializer.Deserialize<PedidoDto>(message) ?? new PedidoDto();
                }
                catch (Exception ex)
                {
                    throw new Exception("Erro deserializar PedidoDto pago", ex);
                }

                var input = new AtualizarStatusPedidoInput(pedidoPago.PedidoId, (int)PedidoStatus.Recebido);
                var command = new AtualizarStatusPedidoCommand(input);
                mediatorHandler.EnviarComando<AtualizarStatusPedidoCommand, PedidoDto?>(command).Wait();
            }
        }
    }
}
