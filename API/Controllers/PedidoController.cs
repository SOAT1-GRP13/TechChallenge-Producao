using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.Pedidos.Commands;
using Application.Pedidos.Boundaries;
using Domain.Base.Communication.Mediator;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;
using Domain.Base.Messages.CommonMessages.Notifications;

namespace API.Controllers
{
    [ApiController]
    [Route("Pedidos")]
    [SwaggerTag("Endpoints relacionados a pedidos, sendo necessário se autenticar")]
    public class PedidoController : ControllerBase
    {
        private readonly IMediatorHandler _mediatorHandler;

        public PedidoController(INotificationHandler<DomainNotification> notifications,
            IMediatorHandler mediatorHandler) : base(notifications, mediatorHandler)
        {
            _mediatorHandler = mediatorHandler;
        }

        [HttpPut("atualizar-status-pedido")]
        [AllowAnonymous]
        [SwaggerOperation(
            Summary = "Atualizar status do pedido",
            Description = "Atualiza o status do pedido, no momento serve como um agnostico ao mercado pago até termos publicado uma url valida para notification_url")]
        [SwaggerResponse(200, "Retorna o pedido atualizado", typeof(PedidoOutput))]
        [SwaggerResponse(404, "Caso não encontre o pedido com o Id informado")]
        [SwaggerResponse(500, "Caso algo inesperado aconteça")]
        public async Task<IActionResult> AtualizarStatusPedido([FromBody] AtualizarStatusPedidoInput input)
        {
            var command = new AtualizarStatusPedidoCommand(input);
            var pedido = await _mediatorHandler.EnviarComando<AtualizarStatusPedidoCommand, PedidoOutput>(command);

            if (!OperacaoValida())
                return StatusCode(StatusCodes.Status400BadRequest, ObterMensagensErro());

            return Ok(pedido);
        }

        [HttpGet("consultar-status-pedido/{pedidoId}")]
        [Authorize]
        [SwaggerOperation(
            Summary = "Consultar status do pedido",
            Description = "Consulta status do pedido a partir do Guid")]
        [SwaggerResponse(200, "Retorna o pedido atualizado", typeof(ConsultarStatusPedidoOutput))]
        [SwaggerResponse(404, "Caso não encontre o pedido com o Id informado")]
        [SwaggerResponse(500, "Caso algo inesperado aconteça")]
        public async Task<IActionResult> ConsultarStatusPedido([FromRoute] Guid pedidoId)
        {
            var command = new ConsultarStatusPedidoCommand(pedidoId);
            var pedido = await _mediatorHandler.EnviarComando<ConsultarStatusPedidoCommand, ConsultarStatusPedidoOutput>(command);

            if (!OperacaoValida())
                return StatusCode(StatusCodes.Status400BadRequest, ObterMensagensErro());

            return Ok(pedido);
        }
    }
}
