using Application.Pedidos.Commands;
using MediatR;
using Domain.Base.Communication.Mediator;
using Domain.Base.Messages.CommonMessages.Notifications;
using Application.Pedidos.Handlers;
using Application.Pedidos.UseCases;
using Application.Pedidos.DTO;
using Application.Common.Interfaces.Services;
using Infra.Services.EmailService;

namespace API.Setup
{
    public static class DependencyInjection
    { 
        public static void RegisterServices(this IServiceCollection services)
        {
            //Mediator
            services.AddScoped<IMediatorHandler, MediatorHandler>();

            //Domain Notifications 
            services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();

            // Notificacao
            services.AddScoped<IPedidoNotificacaoUseCase, PedidoNotificacaoUseCase>();
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<IRequestHandler<NotificaPedidoPagamentoAprovadoCommand, NotificacaoEnviadaPedidoDto?>, NotificaPedidoPagamentoAprovadoCommandHandler>();
            services.AddScoped<IRequestHandler<NotificaPedidoPagamentoReprovadoCommand, NotificacaoEnviadaPedidoDto?>, NotificaPedidoPagamentoReprovadoCommandHandler>();

        }
    }
}
