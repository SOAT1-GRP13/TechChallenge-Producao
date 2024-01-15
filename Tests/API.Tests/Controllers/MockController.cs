using MediatR;
using API.Controllers;
using Domain.Base.Communication.Mediator;
using Domain.Base.Messages.CommonMessages.Notifications;

namespace API.Tests.Controllers
{
    public class MockController : ControllerBase
    {
        public MockController(INotificationHandler<DomainNotification> notifications, IMediatorHandler mediatorHandler) : base(notifications, mediatorHandler)
        {
        }

        public bool OperacaoValida()
        {
            return base.OperacaoValida();
        }

        public IEnumerable<string> ObterMensagensErro()
        {
            return base.ObterMensagensErro();
        }

        public void NotificarErro(string codigo, string mensagem)
        {
            base.NotificarErro(codigo, mensagem);
        }

        public Guid ObterClienteId()
        {
            return base.ObterClienteId();
        }
    }
}
