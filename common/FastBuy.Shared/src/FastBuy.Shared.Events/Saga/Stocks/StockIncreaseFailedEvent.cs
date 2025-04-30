using FastBuy.Shared.Events.Saga.Orders;
using MassTransit;

namespace FastBuy.Shared.Events.Saga.Stocks
{
    class StockIncreaseFailedEvent : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; set; }
        public IEnumerable<OrderItem>? IncreadedItems { get; set; }
        public IEnumerable<OrderItem>? NonIncreadedItems { get; set; }
        public string? Reason { get; set; }
    }
}
