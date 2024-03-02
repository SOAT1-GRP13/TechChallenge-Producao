using MediatR;
using Domain.RabbitMQ;
using Application.Pedidos.DTO;
using Application.Pedidos.Commands;
using Application.Pedidos.UseCases;
using Microsoft.Extensions.Configuration;
using Domain.Base.Communication.Mediator;
using Application.Pedidos.Handlers.Helper;
using Domain.Configuration;
using Microsoft.Extensions.Options;

namespace Application.Pedidos.Handlers
{
    public class PedidoFinalizadoCommandHandler : AlteraStatusPedidoHandleHelper, IRequestHandler<PedidoFinalizadoCommand, PedidoDto?>
    {

 private readonly Secrets _secrets;
        public PedidoFinalizadoCommandHandler(
            IPedidoUseCase statusPedidoUseCase,
            IMediatorHandler mediatorHandler,
            IRabbitMQService rabbitMQService,
            IOptions<Secrets> options
        ) : base(statusPedidoUseCase, mediatorHandler, rabbitMQService) 
        {
            _secrets = options.Value;
         }

        public async Task<PedidoDto?> Handle(PedidoFinalizadoCommand request, CancellationToken cancellationToken)
        {
            return await HandleHelper(request, request.Input, _secrets.ExchangePedidoFinalizado);
        }
    }
}
