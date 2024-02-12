namespace Domain.RabbitMQ
{
    public interface IRabbitMQService
    {
        void PublicaMensagem(string queueName, string message);
    }
}
