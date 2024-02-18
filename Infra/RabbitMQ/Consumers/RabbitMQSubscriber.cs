using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.RabbitMQ.Consumers
{
    public abstract class RabbitMQSubscriber : BackgroundService
    {
        protected readonly IServiceScopeFactory _scopeFactory;
        protected readonly string _nomeDaFila;
        protected readonly IModel _channel;

        public RabbitMQSubscriber(
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

        protected virtual void InvokeReceivedEvent(object? model, BasicDeliverEventArgs ea) { }

        public override void Dispose()
        {
            if (_channel.IsOpen)
                _channel.Close();

            base.Dispose();
        }
    }
}
