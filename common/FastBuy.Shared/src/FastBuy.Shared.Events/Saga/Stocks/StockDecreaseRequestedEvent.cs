using FastBuy.Shared.Events.Saga.Orders;
using MassTransit;

namespace FastBuy.Shared.Events.Saga.Stocks
{
    public record StockDecreaseRequestedEvent : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; set; }
        public required IEnumerable<OrderItem> Items { get; set; }
    }
}
