namespace Application.Pedidos.DTO
{
    public class NotificacaoEnviadaPedidoDto
    {
        public Guid PedidoId { get; set; }
        public string? Email { get; set; }
        public string? Mensagem { get; set; }
        public DateTime? DataEnvio { get; set; }
        public bool? StatusEnvioEmail { get; set; }
    }
}
