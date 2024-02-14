using MediatR;
using Domain.Pedidos;
using Application.Pedidos.DTO;
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

        [HttpPut("pedido-em-preparacao/{id}")]
        [Authorize]
        [SwaggerOperation(
            Summary = "Colocar o pedido Em Preparacao",
            Description = "Atualiza o status do pedido para Em Preparação")]
        [SwaggerResponse(200, "Retorna o pedido atualizado", typeof(PedidoDto))]
        [SwaggerResponse(404, "Caso não encontre o pedido com o Id informado")]
        [SwaggerResponse(500, "Caso algo inesperado aconteça")]
        public async Task<IActionResult> PedidoEmPreparacao([FromRoute] Guid id)
        {
            var input = new AtualizarStatusPedidoInput(id, (int)PedidoStatus.EmPreparacao);
            var command = new PedidoEmPreparacaoCommand(input);
            var pedido = await _mediatorHandler.EnviarComando<PedidoEmPreparacaoCommand, PedidoDto?>(command);

            if (!OperacaoValida())
                return StatusCode(StatusCodes.Status400BadRequest, ObterMensagensErro());

            return Ok(pedido);
        }

        [HttpPut("pedido-pronto/{id}")]
        [Authorize]
        [SwaggerOperation(
            Summary = "Colocar o pedido pronto",
            Description = "Atualiza o status do pedido para Pronto")]
        [SwaggerResponse(200, "Retorna o pedido atualizado", typeof(PedidoDto))]
        [SwaggerResponse(404, "Caso não encontre o pedido com o Id informado")]
        [SwaggerResponse(500, "Caso algo inesperado aconteça")]
        public async Task<IActionResult> PedidoPronto([FromRoute] Guid id)
        {
            var input = new AtualizarStatusPedidoInput(id, (int)PedidoStatus.Pronto);
            var command = new PedidoProntoCommand(input);
            var pedido = await _mediatorHandler.EnviarComando<PedidoProntoCommand, PedidoDto?>(command);

            if (!OperacaoValida())
                return StatusCode(StatusCodes.Status400BadRequest, ObterMensagensErro());

            return Ok(pedido);
        }

        [HttpPut("pedido-finalizado/{id}")]
        [Authorize]
        [SwaggerOperation(
            Summary = "Colocar o pedido como finalizado",
            Description = "Atualiza o status do pedido para Finalizado")]
        [SwaggerResponse(200, "Retorna o pedido atualizado", typeof(PedidoDto))]
        [SwaggerResponse(404, "Caso não encontre o pedido com o Id informado")]
        [SwaggerResponse(500, "Caso algo inesperado aconteça")]
        public async Task<IActionResult> PedidoFinalizado([FromRoute] Guid id)
        {
            var input = new AtualizarStatusPedidoInput(id, (int)PedidoStatus.Finalizado);
            var command = new PedidoFinalizadoCommand(input);
            var pedido = await _mediatorHandler.EnviarComando<PedidoFinalizadoCommand, PedidoDto?>(command);

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
