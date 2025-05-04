using FastBuy.Payments.Api.Persistence.Repository.Abstractions;

namespace FastBuy.Payments.Api.Persistence.UnitOfWork
{
    public interface IPaymentUnitOfWork : IUnitOfWork
    {
        IPaymentRepository PaymentRepository { get; }
        IOrderRepository OrderRepository { get; }
    }
}
