﻿using MediatR;
using Domain.RabbitMQ;
using Application.Pedidos.DTO;
using Application.Pedidos.Commands;
using Application.Pedidos.UseCases;
using Microsoft.Extensions.Configuration;
using Domain.Base.Communication.Mediator;
using Application.Pedidos.Handlers.Helper;

namespace Application.Pedidos.Handlers
{
    public class PedidoFinalizadoCommandHandler : AlteraStatusPedidoHandleHelper, IRequestHandler<PedidoFinalizadoCommand, PedidoDto?>
    {

        public PedidoFinalizadoCommandHandler(
            IPedidoUseCase statusPedidoUseCase,
            IMediatorHandler mediatorHandler,
            IRabbitMQService rabbitMQService,
            IConfiguration configuration
        ) : base(statusPedidoUseCase, mediatorHandler, rabbitMQService, configuration) { }

        public async Task<PedidoDto?> Handle(PedidoFinalizadoCommand request, CancellationToken cancellationToken)
        {
            return await HandleHelper(request, request.Input, "RabbitMQ:ExchangePedidoFinalizado");
        }
    }
}
