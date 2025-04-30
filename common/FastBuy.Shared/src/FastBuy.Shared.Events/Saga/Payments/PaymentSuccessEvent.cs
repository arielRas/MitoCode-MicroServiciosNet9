using MassTransit;

namespace FastBuy.Shared.Events.Saga.Payments
{
    public record PaymentSuccessEvent
    {
        public Guid CorrelationId {  get; set; }
    }
}
