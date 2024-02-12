using System.Text;
using Domain.RabbitMQ;
using Microsoft.AspNetCore.Connections;
using RabbitMQ.Client;

namespace Infra.RabbitMQ
{
    public class RabbitMQService : IRabbitMQService
    {
        private IModel _channel;

        public RabbitMQService(IModel model)
        {
            _channel = model;
        }

        public void PublicaMensagem(string queueName, string message)
        {
            _channel.QueueDeclare(queue: queueName,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(exchange: "",
                                  routingKey: queueName,
                                  basicProperties: null,
                                  body: body);
        }
    }

}
