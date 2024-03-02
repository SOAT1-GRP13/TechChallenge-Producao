using Domain.Pedidos;
using Domain.RabbitMQ;
using System.Text.Json;
using Domain.Base.Messages;
using Application.Pedidos.DTO;
using Domain.Base.DomainObjects;
using Application.Pedidos.UseCases;
using Application.Pedidos.Boundaries;
using Domain.Base.Communication.Mediator;
using Microsoft.Extensions.Configuration;
using Domain.Base.Messages.CommonMessages.Notifications;

namespace Application.Pedidos.Handlers.Helper
{
    public class AlteraStatusPedidoHandleHelper
    {
        private readonly IPedidoUseCase _pedidoUseCase;
        private readonly IMediatorHandler _mediatorHandler;
        private readonly IRabbitMQService _rabbitMQService;

        public AlteraStatusPedidoHandleHelper(
            IPedidoUseCase statusPedidoUseCase,
            IMediatorHandler mediatorHandler,
            IRabbitMQService rabbitMQService
        )
        {
            _pedidoUseCase = statusPedidoUseCase;
            _mediatorHandler = mediatorHandler;
            _rabbitMQService = rabbitMQService;
        }

        public async Task<PedidoDto?> HandleHelper(Command<PedidoDto?> request, AtualizarStatusPedidoInput input, string? nomeExchange)
        {
            if (!request.EhValido())
            {
                foreach (var error in request.ValidationResult.Errors)
                    await _mediatorHandler.PublicarNotificacao(new DomainNotification(request.MessageType, error.ErrorMessage));
                return null;
            }

            try
            {
                var pedidoDto = await _pedidoUseCase.TrocaStatusPedido(input.IdPedido, (PedidoStatus)input.Status);

                if (pedidoDto is null || pedidoDto.PedidoId == Guid.Empty)
                {
                    await _mediatorHandler.PublicarNotificacao(new DomainNotification(request.MessageType, "Pedido não encontrado"));
                    return null;
                }

                if (!string.IsNullOrEmpty(nomeExchange))
                {
                    string mensagem = JsonSerializer.Serialize(pedidoDto);
                    _rabbitMQService.PublicaMensagem(nomeExchange, mensagem);
                }

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
