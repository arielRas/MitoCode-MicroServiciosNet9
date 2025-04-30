using MassTransit;

namespace FastBuy.Shared.Events.Saga.Payments
{
    public record PaymentSuccessEvent : CorrelatedBy<Guid>
    {
        public Guid CorrelationId {  get; set; }
    }
}
