using MassTransit;

namespace FastBuy.Shared.Events.Saga.Orders
{
    public record OrderCreatedEvent : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; set; }
        public Guid OrderId { get; set; }
        public DateTime CreatedAt { get; set; }
        public decimal Amount { get; set; }
    }
}
