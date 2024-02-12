using Domain.RabbitMQ;
using RabbitMQ.Client;
using System.Diagnostics.CodeAnalysis;

namespace Infra.RabbitMQ
{
    [ExcludeFromCodeCoverage]
    public class RabbitMQModelFactory
    {
        private readonly RabbitMQOptions _options;

        public RabbitMQModelFactory(RabbitMQOptions options)
        {
            _options = options;
        }

        public IModel CreateModel()
        {
            var factory = new ConnectionFactory()
            {
                HostName = _options.Hostname,
                Port = _options.Port,
                UserName = _options.Username,
                Password = _options.Password
            };

            var connection = factory.CreateConnection();

            return connection.CreateModel();
        }
    }
}
