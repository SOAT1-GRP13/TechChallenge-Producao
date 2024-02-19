using Domain.Base.DomainObjects;

namespace Domain.Base.Data
{
    public interface IRepository : IDisposable, IAggregateRoot
    {
         IUnitOfWork UnitOfWork { get; }
    }
}