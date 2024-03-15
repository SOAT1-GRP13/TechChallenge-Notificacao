namespace Domain.Configuration
{
    public class Secrets
    {
        public Secrets()
        {

            Rabbit_Hostname = string.Empty;
            Rabbit_Port = "5672";
            Rabbit_Username = string.Empty;
            Rabbit_Password = string.Empty;
            ExchangePedidoPago = string.Empty;
            ExchangePedidoRecusado = string.Empty;
            ExchangePedidoPronto = string.Empty;
            QueuePedidoPago = string.Empty;
            QueuePedidoRecusado = string.Empty;
            Rabbit_VirtualHost = string.Empty;
            QueuePedidoPronto = string.Empty;
        }

        public string Rabbit_Hostname { get; set; }
        public string Rabbit_Port { get; set; }
        public string Rabbit_Username { get; set; }
        public string Rabbit_Password { get; set; }
        public string ExchangePedidoPago { get; set; }
        public string ExchangePedidoRecusado { get; set; }
        public string ExchangePedidoPronto { get; set; }
        public string QueuePedidoPronto { get; set; }
        public string QueuePedidoPago { get; set; }
        public string QueuePedidoRecusado { get; set; }
        public string Rabbit_VirtualHost { get; set; }
    }
}