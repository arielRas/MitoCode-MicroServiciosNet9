namespace FastBuy.Orders.Contracts.Events
{
    public record OrderStateMessage
    {
        public Guid CorrelationId { get; set; }
    }
}
