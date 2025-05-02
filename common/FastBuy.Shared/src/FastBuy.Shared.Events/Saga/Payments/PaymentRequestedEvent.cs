namespace FastBuy.Shared.Events.Saga.Payments
{
    public record PaymentRequestedEvent
    {
        public Guid CorrelationId { get; set; }

        public Guid OrderId { get; set; }

        public decimal Amount { get; set; }
    }
}
