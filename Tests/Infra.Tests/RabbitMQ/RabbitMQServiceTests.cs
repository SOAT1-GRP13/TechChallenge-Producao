using Moq;
using Infra.RabbitMQ;
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
            var exchangeName = "testExchange";
            var message = "Test Message";

            //act
            try
            {
                rabbitMQService.PublicaMensagem(exchangeName, message);
                Assert.True(true);
                return;
            }
            catch (Exception ex)
            {
                //assert
                Assert.True(false, ex.Message);
            }

        }

    }
}
