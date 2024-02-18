using Domain.Base.DomainObjects;

namespace Domain.Pedidos
{
    public class Pedido : Entity, IAggregateRoot
    {
        public int Codigo { get; private set; }
        public Guid ClienteId { get; private set; }
        public DateTime DataCadastro { get; private set; }
        public PedidoStatus PedidoStatus { get; private set; }

        private readonly List<PedidoItem> _pedidoItems;
        public IReadOnlyCollection<PedidoItem> PedidoItems => _pedidoItems;

        public Pedido(Guid clienteId, List<PedidoItem> pedidoItems)
        {
            ClienteId = clienteId;
            _pedidoItems = pedidoItems;
        }

        protected Pedido()
        {
            _pedidoItems = new List<PedidoItem>();
        }

        public void IniciarPedido()
        {
            if (PedidoStatus != PedidoStatus.Iniciado)
                throw new DomainException("Pedido já possui outro status");

            PedidoStatus = PedidoStatus.Iniciado;
        }

        public void CancelarPedido()
        {
            if (PedidoStatus == PedidoStatus.Cancelado)
                throw new DomainException("Pedido já está cancelado");

            if(PedidoStatus == PedidoStatus.Pronto)
                throw new DomainException("Pedido não pode ser cancelado, pois já está pronto");

            if (PedidoStatus == PedidoStatus.EmPreparacao)
                throw new DomainException("Pedido não pode ser cancelado, pois já foi para preparação");

            if (PedidoStatus == PedidoStatus.Finalizado)
                throw new DomainException("Pedido já foi finalizado");

            PedidoStatus = PedidoStatus.Cancelado;
        }

        public void RecusarPedido()
        {
            if (PedidoStatus != PedidoStatus.Iniciado)
                throw new DomainException("Pedido não pode ser recusado, pois não estã com status de recebido");

            PedidoStatus = PedidoStatus.Recusado;
        }

        public void ColocarPedidoComoPronto()
        {
            if (PedidoStatus != PedidoStatus.EmPreparacao)
                throw new DomainException("Pedido não pode ser colocado como pronto, pois o mesmo não está em preparação");

            PedidoStatus = PedidoStatus.Pronto;
        }

        public void ColocarPedidoEmPreparacao()
        {
            if (PedidoStatus != PedidoStatus.Recebido)
                throw new DomainException("Pedido não pode ser colocado em preparação, pois o mesmo não foi recebido");

            PedidoStatus = PedidoStatus.EmPreparacao;
        }

        public void ColocarPedidoComoRecebido()
        {
            if (PedidoStatus == PedidoStatus.Cancelado)
                throw new DomainException("Pedido já está cancelado");

            if (PedidoStatus == PedidoStatus.Pronto)
                throw new DomainException("Pedido não pode ser recebido, pois já está pronto");

            if (PedidoStatus == PedidoStatus.EmPreparacao)
                throw new DomainException("Pedido não pode ser recebido, pois já foi para preparação");

            if (PedidoStatus == PedidoStatus.Finalizado)
                throw new DomainException("Pedido já foi finalizado");

            PedidoStatus = PedidoStatus.Recebido;
        }

        public void FinalizarPedido()
        {
            if (PedidoStatus != PedidoStatus.Pronto)
                throw new DomainException("Pedido não pode ser finalizado, pois não está pronto");

            PedidoStatus = PedidoStatus.Finalizado;
        }

        public void AtualizarStatus(PedidoStatus status)
        {
            switch (status)
            {
                case PedidoStatus.Cancelado:
                    CancelarPedido();
                    break;
                case PedidoStatus.Pronto:
                    ColocarPedidoComoPronto();
                    break;
                case PedidoStatus.EmPreparacao:
                    ColocarPedidoEmPreparacao();
                    break;
                case PedidoStatus.Recebido:
                    ColocarPedidoComoRecebido();
                    break;
                case PedidoStatus.Finalizado:
                    FinalizarPedido();
                    break;
                case PedidoStatus.Recusado:
                    RecusarPedido();
                    break;
                default:
                    throw new DomainException("Status do pedido inválido");
            }
        }
    }
}
