using MassTransit;

namespace FastBuy.Shared.Events.Saga.Payments
{
    public record PaymentRequestedEvent : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; set; }

        public Guid OrderId { get; set; }
    }
}
