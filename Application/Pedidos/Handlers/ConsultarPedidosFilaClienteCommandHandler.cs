using MediatR;
using Domain.Base.DomainObjects;
using Application.Pedidos.Commands;
using Application.Pedidos.UseCases;
using Application.Pedidos.Boundaries;
using Domain.Base.Communication.Mediator;
using Domain.Base.Messages.CommonMessages.Notifications;


namespace Application.Pedidos.Handlers
{
    public class  ConsultarPedidosFilaClienteCommandHandler : IRequestHandler<ConsultarPedidosFilaClienteCommand, IEnumerable<ConsultarPedidosFilaClienteOutput>>
    {
        private readonly IMediatorHandler _mediatorHandler;
        private readonly IPedidoUseCase _pedidoUseCase;

        public ConsultarPedidosFilaClienteCommandHandler(
            IMediatorHandler mediatorHandler,
            IPedidoUseCase pedidoUseCase)
        {
            _mediatorHandler = mediatorHandler;
            _pedidoUseCase = pedidoUseCase;
        }

        public async Task<IEnumerable<ConsultarPedidosFilaClienteOutput>> Handle(ConsultarPedidosFilaClienteCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var pedidosDto = await _pedidoUseCase.ObterPedidosParaFilaDeExibicao();

                var pedidos = new List<ConsultarPedidosFilaClienteOutput>();
                foreach (var pedidoDto in pedidosDto)
                    pedidos.Add(new ConsultarPedidosFilaClienteOutput(pedidoDto.PedidoId, pedidoDto.ClienteId, pedidoDto.PedidoStatus));

                return pedidos;
            }
            catch (DomainException ex)
            {
                await _mediatorHandler.PublicarNotificacao(new DomainNotification(request.MessageType, ex.Message));
            }
            return new List<ConsultarPedidosFilaClienteOutput>();
        }
    }
}
