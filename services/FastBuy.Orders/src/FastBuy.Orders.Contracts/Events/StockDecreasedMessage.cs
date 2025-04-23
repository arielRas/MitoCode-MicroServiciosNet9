namespace FastBuy.Orders.Contracts.Events
{
    public record StockDecreasedMessage
    {
        public Guid CorrelationId { get; set; }
    }
}
