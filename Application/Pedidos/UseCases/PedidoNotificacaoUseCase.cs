using Application.Pedidos.DTO;
using Application.Common.Interfaces.Services;
using Application.Models.Email;

namespace Application.Pedidos.UseCases
{
    public sealed class PedidoNotificacaoUseCase : IPedidoNotificacaoUseCase
    {
        #region Propriedades
        private readonly IEmailSender _emailSender;

        #endregion

        #region Construtor
        public PedidoNotificacaoUseCase(
            IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }
        #endregion

        #region Pedido Notificacao Use Cases
        public async Task<NotificacaoEnviadaPedidoDto> NotificaPagamentoAprovadoPedido(PedidoDto pedidoDto)
        {
            var clientEmail = pedidoDto.ClienteEmail;

            var email = new EmailMessage
            {
                To = clientEmail,
                Body = "O Pagamento do seu pedido foi aprovado"
            };

            var notificacaoEnviada = new NotificacaoEnviadaPedidoDto
            {
                PedidoId = pedidoDto.PedidoId,
                Email = clientEmail,
                Mensagem = email.Body,
                DataEnvio = DateTime.Now,
                StatusEnvioEmail = await _emailSender.SendEmailAsync(email)

            };
            return notificacaoEnviada;

        }

        public async Task<NotificacaoEnviadaPedidoDto> NotificaPagamentoReprovadoPedido(PedidoDto pedidoDto)
        {
            var clientEmail = pedidoDto.ClienteEmail;

            var email = new EmailMessage
            {
                To = clientEmail,
                Body = "O Pagamento do seu pedido foi reprovado"
            };

            var notificacaoEnviada = new NotificacaoEnviadaPedidoDto
            {
                PedidoId = pedidoDto.PedidoId,
                Email = clientEmail,
                Mensagem = email.Body,
                DataEnvio = DateTime.Now,
                StatusEnvioEmail = await _emailSender.SendEmailAsync(email)

            };
            return notificacaoEnviada;
        }

        public async Task<NotificacaoEnviadaPedidoDto> NotificaPedidoPronto(PedidoDto pedidoDto)
        {
            var clientEmail = pedidoDto.ClienteEmail;

            var email = new EmailMessage
            {
                To = clientEmail,
                Body = "O seu pedido está pronto"
            };

            var notificacaoEnviada = new NotificacaoEnviadaPedidoDto
            {
                PedidoId = pedidoDto.PedidoId,
                Email = clientEmail,
                Mensagem = email.Body,
                DataEnvio = DateTime.Now,
                StatusEnvioEmail = await _emailSender.SendEmailAsync(email)

            };
            return notificacaoEnviada;
        }

        #endregion

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
