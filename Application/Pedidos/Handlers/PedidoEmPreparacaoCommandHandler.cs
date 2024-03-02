using MediatR;
using Domain.RabbitMQ;
using Application.Pedidos.DTO;
using Application.Pedidos.Commands;
using Application.Pedidos.UseCases;
using Microsoft.Extensions.Configuration;
using Domain.Base.Communication.Mediator;
using Application.Pedidos.Handlers.Helper;
using Microsoft.AspNetCore.DataProtection;
using Domain.Configuration;
using Microsoft.Extensions.Options;


namespace Application.Pedidos.Handlers
{
    public class PedidoEmPreparacaoCommandHandler : AlteraStatusPedidoHandleHelper, IRequestHandler<PedidoEmPreparacaoCommand, PedidoDto?>
    {
        private readonly Secrets _secrets;

        public PedidoEmPreparacaoCommandHandler(
            IPedidoUseCase statusPedidoUseCase,
            IMediatorHandler mediatorHandler,
            IRabbitMQService rabbitMQService,
            IOptions<Secrets> options
        ) : base(statusPedidoUseCase, mediatorHandler, rabbitMQService)
        {
            _secrets = options.Value;
        }

        public async Task<PedidoDto?> Handle(PedidoEmPreparacaoCommand request, CancellationToken cancellationToken)
        {
            return await HandleHelper(request, request.Input, _secrets.ExchangePedidoPreparando);
        }
    }
}
