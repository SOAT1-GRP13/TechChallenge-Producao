using MediatR;
using Domain.Pedidos;
using Application.Pedidos.DTO;
using Domain.Base.DomainObjects;
using Application.Pedidos.Commands;
using Application.Pedidos.UseCases;
using Domain.Base.Communication.Mediator;
using Domain.Base.Messages.CommonMessages.Notifications;

namespace Application.Pedidos.Handlers
{
    public class AdicionarPedidoCommandHandler : IRequestHandler<AdicionarPedidoCommand, PedidoDto?>
    {
        private readonly IPedidoUseCase _pedidoUseCase;
        private readonly IMediatorHandler _mediatorHandler;

        public AdicionarPedidoCommandHandler(
            IPedidoUseCase statusPedidoUseCase,
            IMediatorHandler mediatorHandler
        )
        {
            _pedidoUseCase = statusPedidoUseCase;
            _mediatorHandler = mediatorHandler;
        }

        public async Task<PedidoDto?> Handle(AdicionarPedidoCommand request, CancellationToken cancellationToken)
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
                var inserido = await _pedidoUseCase.AdicionarPedido(input.pedidoDto);

                if (!inserido)
                {
                    await _mediatorHandler.PublicarNotificacao(new DomainNotification(request.MessageType, "Pedido não inserido"));
                    return null;
                }

                return input.pedidoDto;
            }
            catch (DomainException ex)
            {
                await _mediatorHandler.PublicarNotificacao(new DomainNotification(request.MessageType, ex.Message));
                return null;
            }
        }
    }
}
