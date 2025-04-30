using MassTransit;

namespace FastBuy.Shared.Events.Saga.Stocks
{
    public record StockIncreasedEvent
    {
        public Guid CorrelationId { get; set; }
    }
}
