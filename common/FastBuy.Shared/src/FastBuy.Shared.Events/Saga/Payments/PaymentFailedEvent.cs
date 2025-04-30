using MassTransit;

namespace FastBuy.Shared.Events.Saga.Payments
{
    public record PaymentFailedEvent : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; set; }
        public string? Reason { get; set; }
    }
}
