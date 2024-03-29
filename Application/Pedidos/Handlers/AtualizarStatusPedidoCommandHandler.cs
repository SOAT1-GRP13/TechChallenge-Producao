﻿using MediatR;
using Domain.RabbitMQ;
using Application.Pedidos.DTO;
using Application.Pedidos.Commands;
using Application.Pedidos.UseCases;
using Domain.Base.Communication.Mediator;
using Microsoft.Extensions.Configuration;
using Application.Pedidos.Handlers.Helper;

namespace Application.Pedidos.Handlers
{
    public class AtualizarStatusPedidoCommandHandler : AlteraStatusPedidoHandleHelper, IRequestHandler<AtualizarStatusPedidoCommand, PedidoDto?>
    {

        public AtualizarStatusPedidoCommandHandler(
            IPedidoUseCase statusPedidoUseCase,
            IMediatorHandler mediatorHandler,
            IRabbitMQService rabbitMQService
        ) : base(statusPedidoUseCase, mediatorHandler, rabbitMQService) { }

        public async Task<PedidoDto?> Handle(AtualizarStatusPedidoCommand request, CancellationToken cancellationToken)
        {
            return await HandleHelper(request, request.Input, null);
        }
    }
}
