using MediatR;
using Application.Pedidos.DTO;
using Domain.Base.DomainObjects;
using Application.Pedidos.Commands;
using Application.Pedidos.UseCases;
using Domain.Base.Communication.Mediator;
using Domain.Base.Messages.CommonMessages.Notifications;


namespace Application.Pedidos.Handlers
{
    public class ConsultarPedidosFilaProducaoCommandHandler : IRequestHandler<ConsultarPedidosFilaProducaoCommand, IEnumerable<PedidoDto>>
    {
        private readonly IMediatorHandler _mediatorHandler;
        private readonly IPedidoUseCase _pedidoUseCase;

        public ConsultarPedidosFilaProducaoCommandHandler(
            IMediatorHandler mediatorHandler,
            IPedidoUseCase pedidoUseCase)
        {
            _mediatorHandler = mediatorHandler;
            _pedidoUseCase = pedidoUseCase;
        }

        public async Task<IEnumerable<PedidoDto>> Handle(ConsultarPedidosFilaProducaoCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var pedido = await _pedidoUseCase.ObterPedidosParaFilaDeProducao();

                return pedido;
            }
            catch (DomainException ex)
            {
                await _mediatorHandler.PublicarNotificacao(new DomainNotification(request.MessageType, ex.Message));
            }
            return new List<PedidoDto>();
        }
    }
}
