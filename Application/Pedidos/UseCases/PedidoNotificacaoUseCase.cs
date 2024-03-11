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
            //TODO - get email cliente pelo id no servico de auth
            var clientEmail = "rm348706@fiap.com.br";

            var email = new EmailMessage
            {
                To = clientEmail,
                Body = "O Pagamento do seu pedido foi aprovado"
            };

            var notificacaoEnviada = new NotificacaoEnviadaPedidoDto
            {
                PedidoId = pedidoDto.PedidoId,
                ClienteId = pedidoDto.ClienteId,
                Email = clientEmail,
                Mensagem = email.Body,
                DataEnvio = DateTime.Now,
                StatusEnvioEmail = await _emailSender.SendEmailAsync(email)

            };
            return notificacaoEnviada;

        }

        public async Task<NotificacaoEnviadaPedidoDto> NotificaPagamentoReprovadoPedido(PedidoDto pedidoDto)
        {
            //TODO - get email cliente pelo id no servico de auth
            var clientEmail = "rm348706@fiap.com.br";

            var email = new EmailMessage
            {
                To = clientEmail,
                Body = "O Pagamento do seu pedido foi reprovado"
            };

            var notificacaoEnviada = new NotificacaoEnviadaPedidoDto
            {
                PedidoId = pedidoDto.PedidoId,
                ClienteId = pedidoDto.ClienteId,
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
