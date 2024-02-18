using System.Text;
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
    public class PedidoConfirmadoSubscriber : RabbitMQSubscriber
    {
        public PedidoConfirmadoSubscriber( IServiceScopeFactory scopeFactory, RabbitMQOptions options, IModel model) : base(scopeFactory, options.QueuePedidoConfirmado, model) { } 

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
                    throw new Exception("Erro deserializar PedidoDto confirmado", ex);
                }

                var input = new AdicionarPedidoInput(pedidoPago);
                var command = new AdicionarPedidoCommand(input);
                mediatorHandler.EnviarComando<AdicionarPedidoCommand, PedidoDto?>(command).Wait();
            }
        }
    }
}
