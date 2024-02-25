using System.Text;
using Domain.RabbitMQ;
using RabbitMQ.Client;

namespace Infra.RabbitMQ
{
    public class RabbitMQService : IRabbitMQService
    {
        private readonly IModel _channel;

        public RabbitMQService(IModel model)
        {
            _channel = model;
        }

        public void PublicaMensagem(string exchangeName, string message)
        {
            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(exchange: exchangeName, routingKey: "", basicProperties: null, body: body);
        }
    }

}
