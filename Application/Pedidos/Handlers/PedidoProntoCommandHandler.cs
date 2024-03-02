using MediatR;
using Domain.RabbitMQ;
using Application.Pedidos.DTO;
using Application.Pedidos.Commands;
using Application.Pedidos.UseCases;
using Domain.Base.Communication.Mediator;
using Microsoft.Extensions.Configuration;
using Application.Pedidos.Handlers.Helper;
using Domain.Configuration;
using Microsoft.Extensions.Options;

namespace Application.Pedidos.Handlers
{
    public class PedidoProntoCommandHandler : AlteraStatusPedidoHandleHelper, IRequestHandler<PedidoProntoCommand, PedidoDto?>
    {
        private readonly Secrets _secrets;
        public PedidoProntoCommandHandler(
            IPedidoUseCase statusPedidoUseCase,
            IMediatorHandler mediatorHandler,
            IRabbitMQService rabbitMQService,
            IOptions<Secrets> options
        ) : base(statusPedidoUseCase, mediatorHandler, rabbitMQService)
        {
            _secrets = options.Value;
        }

        public async Task<PedidoDto?> Handle(PedidoProntoCommand request, CancellationToken cancellationToken)
        {
            return await HandleHelper(request, request.Input, _secrets.ExchangePedidoPronto);
        }
    }
}
