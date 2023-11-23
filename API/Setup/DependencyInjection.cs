using Application.Pedidos.Commands;
using Domain.Pedidos;
using Infra.Pedidos.Repository;
using Infra.Pedidos;
using MediatR;
using Domain.Base.Communication.Mediator;
using Domain.Base.Messages.CommonMessages.Notifications;
using Application.Pedidos.Queries;
using Application.Pedidos.Handlers;
using Application.Pedidos.Boundaries;
using Application.Pedidos.UseCases;

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

            // Pedidos
            services.AddScoped<IPedidoRepository, PedidoRepository>();
            services.AddScoped<IPedidoQueries, PedidoQueries>();
            services.AddScoped<IPedidoUseCase, PedidoUseCase>();
            services.AddScoped<PedidosContext>();

            services.AddScoped<IRequestHandler<AtualizarStatusPedidoCommand, PedidoOutput>, AtualizarStatusPedidoCommandHandler>();
            services.AddScoped<IRequestHandler<AdicionarItemPedidoCommand, bool>, AdicionarItemPedidoCommandHandler>();
            services.AddScoped<IRequestHandler<AtualizarItemPedidoCommand, bool>, AtualizarItemPedidoCommandHandler>();
            services.AddScoped<IRequestHandler<RemoverItemPedidoCommand, bool>, RemoverItemPedidoCommandHandler>();
            services.AddScoped<IRequestHandler<IniciarPedidoCommand, ConfirmarPedidoOutput>, IniciarPedidoCommandHandler>();
            services.AddScoped<IRequestHandler<FinalizarPedidoCommand, bool>, FinalizarPedidoCommandHandler>();
            services.AddScoped<IRequestHandler<CancelarProcessamentoPedidoCommand, bool>, CancelarProcessamentoPedidoCommandHandler>();
            services.AddScoped<IRequestHandler<ConsultarStatusPedidoCommand, ConsultarStatusPedidoOutput>, ConsultarStatusPedidoCommandHandler>();
        }
    }
}
