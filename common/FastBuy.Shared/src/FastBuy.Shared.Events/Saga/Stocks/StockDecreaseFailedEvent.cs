using FastBuy.Shared.Events.Saga.Orders;
using MassTransit;

namespace FastBuy.Shared.Events.Saga.Stocks
{
    public record StockDecreaseFailedEvent
    {
        public Guid CorrelationId { get; set; }
        public IEnumerable<OrderItem>? DiscountedItems { get; set; }
        public IEnumerable<OrderItem>? NonDiscountedItems { get; set; }
        public string? Reason { get; set; }
    }
}
