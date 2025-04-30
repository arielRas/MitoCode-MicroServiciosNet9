using MassTransit;

namespace FastBuy.Shared.Events.Saga.Stocks
{
    public record StockDecreasedEvent
    {
        public Guid CorrelationId { get; set; }
    }
}
