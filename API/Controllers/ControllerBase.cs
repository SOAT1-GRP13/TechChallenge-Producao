using MediatR;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Domain.Base.DomainObjects;
using Domain.Base.Communication.Mediator;
using Domain.Base.Messages.CommonMessages.Notifications;

namespace API.Controllers
{
    public abstract class ControllerBase : Controller
    {
        private readonly DomainNotificationHandler _notifications;
        private readonly IMediatorHandler _mediatorHandler;

        protected ControllerBase(INotificationHandler<DomainNotification> notifications,
                                 IMediatorHandler mediatorHandler)
        {
            _notifications = (DomainNotificationHandler)notifications;
            _mediatorHandler = mediatorHandler;
        }

        protected bool OperacaoValida()
        {
            return !_notifications.TemNotificacao(); // se tem alguma notificacao de problema, retorna operacao invalida.
        }

        protected IEnumerable<string> ObterMensagensErro()
        {
            return _notifications.ObterNotificacoes().Select(c => c.Value).ToList();
        }

        protected void NotificarErro(string codigo, string mensagem)
        {
            _mediatorHandler.PublicarNotificacao(new DomainNotification(codigo, mensagem));
        }

        protected Guid ObterClienteId()
        {

            if (!string.IsNullOrEmpty(User.FindFirstValue(ClaimTypes.NameIdentifier)))
                return Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            throw new DomainException("Cliente não identificado");
        }
    }
}
