using FastBuy.Shared.Events.Saga.Orders;

namespace FastBuy.Shared.Events.Saga.Stocks
{
    public record StockIncreaseRequestedEvent
    {        
        public Guid CorrelationId { get; set; }
        public required IEnumerable<OrderItem> Items { get; set; }
    }
}
