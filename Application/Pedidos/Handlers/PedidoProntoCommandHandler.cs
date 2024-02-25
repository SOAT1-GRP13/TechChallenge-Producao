using MediatR;
using Domain.RabbitMQ;
using Application.Pedidos.DTO;
using Application.Pedidos.Commands;
using Application.Pedidos.UseCases;
using Domain.Base.Communication.Mediator;
using Microsoft.Extensions.Configuration;
using Application.Pedidos.Handlers.Helper;

namespace Application.Pedidos.Handlers
{
    public class PedidoProntoCommandHandler : AlteraStatusPedidoHandleHelper, IRequestHandler<PedidoProntoCommand, PedidoDto?>
    {
        public PedidoProntoCommandHandler(
            IPedidoUseCase statusPedidoUseCase,
            IMediatorHandler mediatorHandler,
            IRabbitMQService rabbitMQService,
            IConfiguration configuration
        ) : base (statusPedidoUseCase, mediatorHandler, rabbitMQService, configuration) { }

        public async Task<PedidoDto?> Handle(PedidoProntoCommand request, CancellationToken cancellationToken)
        {
            return await HandleHelper(request, request.Input, "RabbitMQ:ExchangePedidoPronto");
        }
    }
}
