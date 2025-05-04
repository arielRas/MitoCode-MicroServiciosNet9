using FastBuy.Payments.Api.Persistence.Repository.Abstractions;
using FastBuy.Payments.Api.Persistence.Repository.Implementations;
using Microsoft.EntityFrameworkCore.Storage;

namespace FastBuy.Payments.Api.Persistence.UnitOfWork
{
    public class PaymentUnitOfWork : IPaymentUnitOfWork
    {
        private readonly PaymentsDbContext _context;        
        private readonly IPaymentRepository _paymentRepository;
        private readonly IOrderRepository _orderRepository;
        private IDbContextTransaction? _transaction;

        public PaymentUnitOfWork(PaymentsDbContext context)
        {
            _context = context;
            _paymentRepository = new PaymentRepository(_context);
            _orderRepository = new OrderRepository(_context);
        }

        public IPaymentRepository PaymentRepository => _paymentRepository;

        public IOrderRepository OrderRepository => _orderRepository;

        public async Task BeginTransactionAsync()
            => _transaction ??= await _context.Database.BeginTransactionAsync();

        public async Task CommitTransactionAsync()
        {
            if (IsTransactionActive())
            {
                await _context.SaveChangesAsync();
                await _transaction!.CommitAsync();
                _transaction?.Dispose();
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (IsTransactionActive())
            {
                await _transaction!.RollbackAsync();
                _transaction?.Dispose();
                _transaction = null;
            }
        }

        private bool IsTransactionActive()
            => _transaction is not null && _transaction.GetDbTransaction().Connection is not null;
    }
}
