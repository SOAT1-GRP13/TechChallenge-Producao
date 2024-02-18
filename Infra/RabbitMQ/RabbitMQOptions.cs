namespace Infra.RabbitMQ
{
    public class RabbitMQOptions
    {
        public RabbitMQOptions() { 
            Hostname = string.Empty;
            Port = 5672;
            Username = string.Empty;
            Password = string.Empty;
            QueuePedidoConfirmado = string.Empty;
            QueuePedidoPago = string.Empty;
            QueuePedidoPreparando = string.Empty;
            QueuePedidoPronto = string.Empty;
            VirtualHost = string.Empty;
        }

        public string Hostname { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string QueuePedidoConfirmado { get; set; }
        public string QueuePedidoPago { get; set; }
        public string QueuePedidoPreparando { get; set; }
        public string QueuePedidoPronto { get; set; }
        public string VirtualHost {get;set;}
    }
}
