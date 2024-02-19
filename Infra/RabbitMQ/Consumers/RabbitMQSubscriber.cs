using System.Text;
using RabbitMQ.Client;
using System.Text.Json;
using RabbitMQ.Client.Events;
using Application.Pedidos.DTO;
using Domain.Base.DomainObjects;
using Microsoft.Extensions.Hosting;
using Domain.Base.Communication.Mediator;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.RabbitMQ.Consumers
{
    public abstract class RabbitMQSubscriber : BackgroundService
    {
        protected readonly IServiceScopeFactory _scopeFactory;
        protected readonly string _nomeDaFila;
        protected readonly IModel _channel;

        protected RabbitMQSubscriber(
            IServiceScopeFactory scopeFactory,
            string nomeFila,
            IModel model)
        {
            _scopeFactory = scopeFactory;
            _nomeDaFila = nomeFila;
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

        protected virtual void InvokeReceivedEvent(object? model, BasicDeliverEventArgs ea) 
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var mediatorHandler = scope.ServiceProvider.GetRequiredService<IMediatorHandler>();

                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                PedidoDto pedidoDto;
                try
                {
                    pedidoDto = JsonSerializer.Deserialize<PedidoDto>(message) ?? new PedidoDto();
                }
                catch (Exception ex)
                {
                    throw new DomainException("Erro deserializar PedidoDto", ex);
                }

                InvokeCommand(pedidoDto, mediatorHandler);
            }
        }

        protected virtual void InvokeCommand(PedidoDto pedidoDto, IMediatorHandler mediatorHandler) { }

        public override void Dispose()
        {
            if (_channel.IsOpen)
                _channel.Close();

            GC.SuppressFinalize(this);

            base.Dispose();
        }
    }
}
