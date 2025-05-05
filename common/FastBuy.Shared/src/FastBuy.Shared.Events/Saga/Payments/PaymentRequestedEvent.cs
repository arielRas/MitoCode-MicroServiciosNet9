namespace FastBuy.Shared.Events.Saga.Payments
{
    public record PaymentRequestedEvent
    {
        public Guid CorrelationId { get; set; }
    }
}
