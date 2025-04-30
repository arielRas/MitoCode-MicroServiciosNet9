using MassTransit;

namespace FastBuy.Shared.Events.Saga.Stocks
{
    public record StockDecreasedEvent : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; set; }
    }
}
