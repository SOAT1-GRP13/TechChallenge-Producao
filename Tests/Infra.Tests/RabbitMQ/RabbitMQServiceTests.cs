using Domain.RabbitMQ;
using Infra.RabbitMQ;
using Moq;
using System.Text;
using RabbitMQ.Client;

namespace Infra.Tests.RabbitMQ
{
    public class RabbitMQServiceTests
    {
        [Fact]
        public void AoPublicaMensagem_DevePublicarMensagemNaFila()
        {
            //arrange
            var mockModel = new Mock<IModel>();

            var rabbitMQService = new RabbitMQService(mockModel.Object);
            var queueName = "testQueue";
            var message = "Test Message";
            var messageBody = Encoding.UTF8.GetBytes(message);

            //act
            rabbitMQService.PublicaMensagem(queueName, message);

            //assert
            mockModel.Verify(ch => ch.QueueDeclare(queueName, true, false, false, null), Times.Once);
        }

    }
}
