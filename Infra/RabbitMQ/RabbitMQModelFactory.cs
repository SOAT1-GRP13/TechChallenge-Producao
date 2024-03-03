using Domain.Configuration;
using Domain.RabbitMQ;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Diagnostics.CodeAnalysis;

namespace Infra.RabbitMQ
{
    [ExcludeFromCodeCoverage]
    public class RabbitMQModelFactory
    {
        private readonly Secrets _options;

        public RabbitMQModelFactory(IOptions<Secrets> options)
        {
            _options = options.Value;
        }

        public IModel CreateModel()
        {
            var factory = new ConnectionFactory()
            {
                HostName = _options.Rabbit_Hostname,
                Port = Convert.ToInt32(_options.Rabbit_Port),
                UserName = _options.Rabbit_Username,
                Password = _options.Rabbit_Password,
                VirtualHost = _options.Rabbit_VirtualHost
            };

            var connection = factory.CreateConnection();

            return connection.CreateModel();
        }
    }
}
