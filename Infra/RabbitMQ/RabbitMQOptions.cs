namespace Infra.RabbitMQ
{
    public class RabbitMQOptions
    {
        public RabbitMQOptions() { 
            Hostname = string.Empty;
            Port = 5672;
            Username = string.Empty;
            Password = string.Empty;
            ExchangePedidoConfirmado = string.Empty;
            ExchangePedidoPago = string.Empty;
            ExchangePedidoRecusado = string.Empty;
            ExchangePedidoPreparando = string.Empty;
            ExchangePedidoPronto = string.Empty;
            ExchangePedidoFinalizado = string.Empty;
            QueuePedidoConfirmado = string.Empty;
            QueuePedidoPago = string.Empty;
            QueuePedidoRecusado = string.Empty;
            VirtualHost = string.Empty;
        }

        public string Hostname { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ExchangePedidoConfirmado { get; set; }        
        public string ExchangePedidoPago { get; set; }        
        public string ExchangePedidoRecusado { get; set; }        
        public string ExchangePedidoPreparando { get; set; }        
        public string ExchangePedidoPronto { get; set; }
        public string ExchangePedidoFinalizado { get; set; }
        public string QueuePedidoConfirmado { get; set; }
        public string QueuePedidoPago { get; set; }
        public string QueuePedidoRecusado { get; set; }
        public string VirtualHost {get;set;}
    }
}
