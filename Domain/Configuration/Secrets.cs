namespace Domain.Configuration
{
    public class Secrets
    {
        public Secrets()
        {
            MercadoPagoUserId = string.Empty;
            AccesToken = string.Empty;
            Notification_url = string.Empty;
            External_Pos_Id = string.Empty;
            ClientSecret = string.Empty;
            PreSalt = string.Empty;
            PosSalt = string.Empty;
            ConnectionString = string.Empty;

            Rabbit_Hostname = string.Empty;
            Rabbit_Port = "5672";
            Rabbit_Username = string.Empty;
            Rabbit_Password = string.Empty;
            ExchangePedidoConfirmado = string.Empty;
            ExchangePedidoPago = string.Empty;
            ExchangePedidoRecusado = string.Empty;
            ExchangePedidoPreparando = string.Empty;
            ExchangePedidoPronto = string.Empty;
            ExchangePedidoFinalizado = string.Empty;
            QueuePedidoConfirmado = string.Empty;
            QueuePedidoPago = string.Empty;
            QueuePedidoRecusado = string.Empty;
            Rabbit_VirtualHost = string.Empty;
        }

        public string MercadoPagoUserId { get; set; }
        public string AccesToken { get; set; }
        public string Notification_url { get; set; }
        public string External_Pos_Id { get; set; }
        public string ClientSecret { get; set; }
        public string PreSalt { get; set; }
        public string PosSalt { get; set; }
        public string ConnectionString { get; set; }

        public string Rabbit_Hostname { get; set; }
        public string Rabbit_Port { get; set; }
        public string Rabbit_Username { get; set; }
        public string Rabbit_Password { get; set; }
        public string ExchangePedidoConfirmado { get; set; }
        public string ExchangePedidoPago { get; set; }
        public string ExchangePedidoRecusado { get; set; }
        public string ExchangePedidoPreparando { get; set; }
        public string ExchangePedidoPronto { get; set; }
        public string ExchangePedidoFinalizado { get; set; }
        public string QueuePedidoConfirmado { get; set; }
        public string QueuePedidoPago { get; set; }
        public string QueuePedidoRecusado { get; set; }
        public string Rabbit_VirtualHost { get; set; }
    }
}