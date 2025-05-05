namespace FastBuy.Shared.Events.Saga.Payments
{
    public record PaymentFailedEvent
    {
        public Guid CorrelationId { get; set; }
        public string? Reason { get; set; }
    }
}
