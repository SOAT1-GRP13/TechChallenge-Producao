using MediatR;
using Domain.Pedidos;
using Domain.RabbitMQ;
using System.Text.Json;
using Application.Pedidos.DTO;
using Domain.Base.DomainObjects;
using Application.Pedidos.Commands;
using Application.Pedidos.UseCases;
using Domain.Base.Communication.Mediator;
using Microsoft.Extensions.Configuration;
using Domain.Base.Messages.CommonMessages.Notifications;

namespace Application.Pedidos.Handlers
{
    public class PedidoProntoCommandHandler : IRequestHandler<PedidoProntoCommand, PedidoDto?>
    {
        private readonly IPedidoUseCase _pedidoUseCase;
        private readonly IMediatorHandler _mediatorHandler;
        private readonly IRabbitMQService _rabbitMQService;
        private readonly IConfiguration _configuration;

        public PedidoProntoCommandHandler(
            IPedidoUseCase statusPedidoUseCase,
            IMediatorHandler mediatorHandler,
            IRabbitMQService rabbitMQService,
            IConfiguration configuration
        )
        {
            _pedidoUseCase = statusPedidoUseCase;
            _mediatorHandler = mediatorHandler;
            _rabbitMQService = rabbitMQService;
            _configuration = configuration;
        }

        public async Task<PedidoDto?> Handle(PedidoProntoCommand request, CancellationToken cancellationToken)
        {
            if (!request.EhValido())
            {
                foreach (var error in request.ValidationResult.Errors)
                    await _mediatorHandler.PublicarNotificacao(new DomainNotification(request.MessageType, error.ErrorMessage));
                return null;
            }

            try
            {
                var input = request.Input;
                var pedidoDto = await _pedidoUseCase.TrocaStatusPedido(input.IdPedido, (PedidoStatus)input.Status);

                if (pedidoDto is null)
                {
                    await _mediatorHandler.PublicarNotificacao(new DomainNotification(request.MessageType, "Pedido não encontrado"));
                    return null;
                }

                string mensagem = JsonSerializer.Serialize(pedidoDto);
                var fila = _configuration.GetSection("RabbitMQ:QueuePedidoPronto").Value;
                _rabbitMQService.PublicaMensagem(fila, mensagem);

                return pedidoDto;
            }
            catch (DomainException ex)
            {
                await _mediatorHandler.PublicarNotificacao(new DomainNotification(request.MessageType, ex.Message));
                return null;
            }
        }
    }
}
