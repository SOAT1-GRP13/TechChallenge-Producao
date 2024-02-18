using System.Text;
using Domain.Pedidos;
using RabbitMQ.Client;
using System.Text.Json;
using RabbitMQ.Client.Events;
using Application.Pedidos.DTO;
using Microsoft.Extensions.Hosting;
using Application.Pedidos.Commands;
using Application.Pedidos.Boundaries;
using Domain.Base.Communication.Mediator;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.RabbitMQ.Consumers
{
    public class PedidoPagoSubscriber : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly string _nomeDaFila;
        private IModel _channel;

        public PedidoPagoSubscriber(
            IServiceScopeFactory scopeFactory,
            RabbitMQOptions options,
            IModel model)
        {
            _scopeFactory = scopeFactory;
            _nomeDaFila = options.QueuePedidoPago;

            _channel = model;
            _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
            _channel.QueueDeclare(queue: _nomeDaFila, durable: true, exclusive: false, autoDelete: false, arguments: null);
            _channel.QueueBind(queue: _nomeDaFila,
                exchange: "trigger",
                routingKey: "");
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (ModuleHandle, ea) => { InvokeReceivedEvent(ModuleHandle, ea); };

            _channel.BasicConsume(queue: _nomeDaFila, autoAck: true, consumer: consumer);

            return Task.CompletedTask;
        }

        protected void InvokeReceivedEvent(object? model, BasicDeliverEventArgs ea)
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
                    throw new Exception("Erro deserializar PedidoDto", ex);
                }

                var input = new AdicionarPedidoInput(pedidoPago);
                var command = new AdicionarPedidoCommand(input);
                mediatorHandler.EnviarComando<AdicionarPedidoCommand, PedidoDto?>(command).Wait();
            }
        }

        public override void Dispose()
        {
            if (_channel.IsOpen)
                _channel.Close();

            base.Dispose();
        }
    }
}
