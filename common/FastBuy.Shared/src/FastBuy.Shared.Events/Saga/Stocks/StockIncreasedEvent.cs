using MassTransit;

namespace FastBuy.Shared.Events.Saga.Stocks
{
    public record StockIncreasedEvent : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; set; }
    }
}
