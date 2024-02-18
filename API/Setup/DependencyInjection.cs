using Application.Pedidos.Commands;
using Domain.Pedidos;
using Infra.Pedidos.Repository;
using Infra.Pedidos;
using MediatR;
using Domain.Base.Communication.Mediator;
using Domain.Base.Messages.CommonMessages.Notifications;
using Application.Pedidos.Handlers;
using Application.Pedidos.Boundaries;
using Application.Pedidos.UseCases;
using Application.Pedidos.DTO;

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
            services.AddScoped<IPedidoUseCase, PedidoUseCase>();
            services.AddScoped<PedidosContext>();

            services.AddScoped<IRequestHandler<AdicionarPedidoCommand, PedidoDto?>, AdicionarPedidoCommandHandler>();
            services.AddScoped<IRequestHandler<AtualizarStatusPedidoCommand, PedidoDto?>, AtualizarStatusPedidoCommandHandler>();
            services.AddScoped<IRequestHandler<PedidoEmPreparacaoCommand, PedidoDto?>, PedidoEmPreparacaoCommandHandler>();
            services.AddScoped<IRequestHandler<PedidoProntoCommand, PedidoDto?>, PedidoProntoCommandHandler>();
            services.AddScoped<IRequestHandler<PedidoFinalizadoCommand, PedidoDto?>, PedidoFinalizadoCommandHandler>();
            services.AddScoped<IRequestHandler<ConsultarStatusPedidoCommand, ConsultarStatusPedidoOutput>, ConsultarStatusPedidoCommandHandler>();
            services.AddScoped<IRequestHandler<ConsultarPedidosFilaProducaoCommand, IEnumerable<PedidoDto>>, ConsultarPedidosFilaProducaoCommandHandler>();
            services.AddScoped<IRequestHandler<ConsultarPedidosFilaClienteCommand, IEnumerable<ConsultarPedidosFilaClienteOutput>>, ConsultarPedidosFilaClienteCommandHandler>();
        }
    }
}
