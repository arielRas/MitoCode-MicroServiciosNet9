using FastBuy.Shared.Events.Saga.Orders;

namespace FastBuy.Shared.Events.Saga.Stocks
{
    public record StockIncreaseFailedEvent
    {
        public Guid CorrelationId { get; set; }
        public IEnumerable<OrderItem>? IncreadedItems { get; set; }
        public IEnumerable<OrderItem>? NonIncreadedItems { get; set; }
        public string? Reason { get; set; }
    }
}
